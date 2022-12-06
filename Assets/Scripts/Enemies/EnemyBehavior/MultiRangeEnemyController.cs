using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MultiRangeEnemyState
{
    Idle,
    Wander,
    Hit,
    MeleeAlert,
    AttackDash,
    MeleeAttack,
    BackDash,
    RangedAlert,
    RangedActive,
    RangedAttack,
    RangedConfused,
    DeadLaunch,
    Dead
}

public class MultiRangeEnemyController : MonoBehaviour
{
    EnemyAnimator enemyAnimator;
    EnemyData enemyData;
    EnemySound enemySounds;
    EnemyParticles enemyParticles;
    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    ProjectilePool projectiles;

    public GameObject chaseTarget;
    public GameObject animationProps;

    [SerializeField]
    MultiRangeEnemyState currentState;

    [SerializeField]
    float rangedAttackTimer;
    float meleeAttackTimer;
    float wanderTimer;

    bool canMove = true;
    bool canMeleeAttack = false;
    bool canRangedAttack = false;
    bool shouldWander = false;
    bool shouldAttack = false;
    bool shouldHitStun = false;
    bool targetChosen = false;
    bool isGrounded = true;

    [SerializeField]
    Vector2 currentWanderTarget;
    float currentWanderMin;
    float currentWanderMax;
    public float lastWanderDistance;

    public bool isFacingRight;

    public float DashSpeed;
    public float WanderSpeed;
    [Header("The range the player has to enter for the enemy to start shooting (X axis only)")]
    public float xShootingDistance;
    [Header("The range the player has to enter for the enemy to dash to them")]
    public float MeleeStateActivationDistance;
    [Header("How far away the enemy is from the player when it attacks")]
    public float MeleeAttackDistance;
    public float MeleeAttackCooldown;
    public float RangedAttackCooldown;
    [Header("How fast the tengu moves up and down to match the player's Y axis when shooting")]
    public float RangedYMoveSpeed;
    public float WanderCooldown;
    public int maxWanderDistance;
    public bool willWander = true;

    public Vector2 deathLaunchDistance;
    private Vector2 returnDashLocation;
    private Vector2 hitLocation;

    bool groundedLastFrame = false;

    private LayerMask groundMask;
    private LayerMask platformMask;

    public float groundCheckXDistance = 0.25f;
    public float groundCheckYDistance = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
        enemyParticles = GetComponent<EnemyParticles>();
        enemySounds = GetComponent<EnemySound>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();

        chaseTarget = GameObject.FindWithTag("Player");
        currentState = MultiRangeEnemyState.Idle;

        rangedAttackTimer = RangedAttackCooldown;
        meleeAttackTimer = MeleeAttackCooldown;
        wanderTimer = WanderCooldown;

        groundMask = LayerMask.GetMask("Ground");
        platformMask = LayerMask.GetMask("One-Way Platform");
    }

    private void FixedUpdate()
    {
        if (currentState != MultiRangeEnemyState.Dead && currentState != MultiRangeEnemyState.DeadLaunch)
        {
            if (currentState != MultiRangeEnemyState.Hit && currentState != MultiRangeEnemyState.RangedAlert && currentState != MultiRangeEnemyState.MeleeAlert)
            {
                DetermineIfShouldShoot();
                DetermineIfShouldMelee();
                DetermineState();
            }

            UpdateTimers();
            DoState(currentState);
        }
        UpdateGrounding();
    }

    public void SwitchState(MultiRangeEnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case (MultiRangeEnemyState.Idle):
                enemyAnimator.SwitchState(MultiRangeEnemyState.Idle);
                wanderTimer = WanderCooldown;
                canMove = true;
                break;
            case (MultiRangeEnemyState.Wander):
                enemyAnimator.SwitchState(MultiRangeEnemyState.Wander);
                DetermineWanderBoundaries();
                DetermineWanderDistance();
                break;
            case (MultiRangeEnemyState.AttackDash):
                meleeAttackTimer = MeleeAttackCooldown;
                returnDashLocation = this.transform.position;
                break;
            case (MultiRangeEnemyState.RangedActive):
                rangedAttackTimer = RangedAttackCooldown;
                break;
        }
    }

    public void DoState(MultiRangeEnemyState state)
    {
        switch (state)
        {
            case (MultiRangeEnemyState.Idle):
                canMove = true;
                break;
            case (MultiRangeEnemyState.Wander):
                Wander();
                break;
            case (MultiRangeEnemyState.AttackDash):
                DashToPlayer();
                break;
            case (MultiRangeEnemyState.RangedActive):
                ShootAtPlayer();
                break;
        }
    }

    void DetermineWanderBoundaries()
    {
        currentWanderMin = transform.position.x - maxWanderDistance;
        currentWanderMax = transform.position.x + maxWanderDistance;
    }

    void DetermineIfShouldWander()
    {
        if (shouldWander && willWander && currentState == MultiRangeEnemyState.Idle && currentState != MultiRangeEnemyState.RangedConfused)
        {
            SwitchState(MultiRangeEnemyState.Wander);
        }
    }

    void DetermineIfShouldShoot()
    {
        if (currentState != MultiRangeEnemyState.RangedAttack && currentState != MultiRangeEnemyState.AttackDash)
        {
            float xTargetDistance = Mathf.Abs(transform.position.x - chaseTarget.transform.position.x);

            if (xTargetDistance <= xShootingDistance && xTargetDistance >= MeleeAttackDistance)
            {
                Debug.Log("Start shooting");
            }
        }
    }

    void DetermineIfShouldMelee()
    {
        if (currentState != MultiRangeEnemyState.MeleeAttack && currentState != MultiRangeEnemyState.RangedActive)
        {
            float xTargetDistance = Mathf.Abs(transform.position.x - chaseTarget.transform.position.x);

            if (xTargetDistance <= MeleeStateActivationDistance)
            {
                Debug.Log("Start dashing");
                SwitchState(MultiRangeEnemyState.AttackDash);
            }
        }
    }

    void DetermineWanderDistance()
    {
        if (!targetChosen)
        {
            float wanderDirection = Random.Range(0, 2);

            // Goes right if random was 1, goes left if random was 0
            if (wanderDirection == 0)
            {
                currentWanderTarget = new Vector2(currentWanderMin, transform.position.y);
            }
            else
            {
                currentWanderTarget = new Vector2(currentWanderMax, transform.position.y);
            }

            targetChosen = true;
        }
    }

    void DetermineState()
    {
        DetermineIfShouldWander();

        if (shouldHitStun)
        {
            SwitchState(MultiRangeEnemyState.Hit);
            shouldHitStun = false;
        }
    }

    void DashToPlayer()
    {
        if (transform.position.x < chaseTarget.transform.position.x && !isFacingRight)
        {
            FlipEnemy();
        }

        if (transform.position.x > chaseTarget.transform.position.x && isFacingRight)
        {
            FlipEnemy();
        }

        Vector2 currentMovement = Vector2.MoveTowards(transform.position, chaseTarget.transform.position, DashSpeed * Time.deltaTime);
        float targetDistance = Vector2.Distance(transform.position, chaseTarget.transform.position);

        if (targetDistance <= MeleeAttackDistance && canMeleeAttack)
        {
            SwitchState(MultiRangeEnemyState.MeleeAttack);
            return;
        }

        if (targetDistance >= MeleeStateActivationDistance)
        {
            SwitchState(MultiRangeEnemyState.Idle);
        } else
        {
            transform.position = currentMovement;
        }
    }

    void ShootAtPlayer()
    {
        Vector2 currentMovement = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, chaseTarget.transform.position.y), RangedYMoveSpeed * Time.deltaTime);
        float xTargetDistance = Mathf.Abs(transform.position.x - chaseTarget.transform.position.x);

        if (canRangedAttack)
        {
            SwitchState(MultiRangeEnemyState.RangedAttack);
            return;
        }

        if (xTargetDistance >= MeleeStateActivationDistance)
        {
            SwitchState(MultiRangeEnemyState.Idle);
        }
        else
        {
            transform.position = currentMovement;
        }
    }

    void TurnAround()
    {
        if (currentState == MultiRangeEnemyState.Wander)
        {
            //Debug.Log("Turning Around");
            if (isFacingRight)
            {
                currentWanderTarget = new Vector2(currentWanderMin, transform.position.y);
            }
            else
            {
                currentWanderTarget = new Vector2(currentWanderMax, transform.position.y);
            }
        }
    }

    void Wander()
    {
        if (lastWanderDistance >= -0.1f && maxWanderDistance <= 0.1f)
        {
            Debug.Log("Enemy is being flipped");
            FlipEnemy();
        }

        if (transform.position.x < currentWanderTarget.x && !isFacingRight)
        {
            FlipEnemy();
        }

        if (transform.position.x > currentWanderTarget.x && isFacingRight)
        {
            FlipEnemy();
        }

        Vector2 currentMovement = Vector2.MoveTowards(transform.position, currentWanderTarget, WanderSpeed * Time.deltaTime);
        lastWanderDistance = currentMovement.x - transform.position.x;
        transform.position = currentMovement;

        if ((Vector2)transform.position == currentWanderTarget)
        {
            targetChosen = false;
            shouldWander = false;
            wanderTimer = WanderCooldown;
            SwitchState(MultiRangeEnemyState.Idle);
        }
    }

    void MeleeAttackMode()
    {

    }

    void MeleeDashBack()
    {

    }

    private void FlipEnemy()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;

        Vector3 propsLocalScale = animationProps.transform.localScale;
        propsLocalScale.x *= -1f;
        animationProps.transform.localScale = propsLocalScale;
    }

    void ResetWander() =>
        shouldWander = true;

    void ResetMeleeAttack() =>
        canMeleeAttack = true;

    void ResetRangedAttack() =>
        canRangedAttack = true;

    private void UpdateTimers()
    {
        UpdateMeleeAttackTimer();
        UpdateWanderTimer();
        UpdateRangedAttackTimer();
    }

    void UpdateWanderTimer()
    {
        if (wanderTimer > 0)
        {
            wanderTimer -= 1 * Time.deltaTime;

            if (wanderTimer <= 0)
            {
                wanderTimer = 0f;
                ResetWander();
            }
        }
    }

    void UpdateMeleeAttackTimer()
    {
        if (meleeAttackTimer > 0)
        {
            meleeAttackTimer -= 1 * Time.deltaTime;

            if (meleeAttackTimer <= 0)
            {
                meleeAttackTimer = 0f;
                ResetMeleeAttack();
            }
        }
    }

    void UpdateRangedAttackTimer()
    {
        if (rangedAttackTimer > 0)
        {
            rangedAttackTimer -= 1 * Time.deltaTime;

            if (rangedAttackTimer <= 0)
            {
                rangedAttackTimer = 0f;
                ResetRangedAttack();
            }
        }
    }

    void UpdateGrounding()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(collider.bounds.min.x + groundCheckXDistance, collider.bounds.min.y), new Vector2(collider.bounds.max.x - groundCheckXDistance, collider.bounds.min.y - groundCheckYDistance), groundMask)
            || Physics2D.OverlapArea(new Vector2(collider.bounds.min.x + groundCheckXDistance, collider.bounds.min.y), new Vector2(collider.bounds.max.x - groundCheckXDistance, collider.bounds.min.y - groundCheckYDistance), platformMask);

        if (isGrounded && currentState == MultiRangeEnemyState.DeadLaunch && !groundedLastFrame)
        {
            SwitchState(MultiRangeEnemyState.Dead);
            enemyParticles.PlayLandParticles();
        }

        groundedLastFrame = isGrounded;
    }

    private void AnimationEndTransitionToNextState(MultiRangeEnemyState nextState)
    {
        if (enemyAnimator.CheckIfAnimationEnded())
        {
            SwitchState(nextState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox") && (currentState != MultiRangeEnemyState.Dead && currentState != MultiRangeEnemyState.DeadLaunch))
        {
            SwitchState(MultiRangeEnemyState.Hit);
            hitLocation = collision.ClosestPoint(new Vector2(transform.position.x, transform.position.y));
        }

        if (collision.gameObject.CompareTag("Bounds"))
        {
            TurnAround();
        }

        if (collision.gameObject.CompareTag("Bumperbox") && currentState == MultiRangeEnemyState.Wander)
        {
            TurnAround();
        }
    }
}

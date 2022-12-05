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

    public float AttackDistance;
    public float ChaseSpeed;
    public float WanderSpeed;
    public float xShootingDistance;
    public float MeleeAttackDistance;
    public float MeleeAttackCooldown;
    public float RangedAttackCooldown;
    public float WanderCooldown;
    public int maxWanderDistance;
    public bool willWander = true;

    public Vector2 deathLaunchDistance;
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

    }

    void DetermineIfShouldMelee()
    {

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

    void RangedAttackMode()
    {

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

        if (isGrounded)
        {
            rigidbody.gravityScale = 1f;
        }
        else
        {
            if (currentState != MultiRangeEnemyState.Hit)
            {
                rigidbody.gravityScale = 5f;
            }
        }

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

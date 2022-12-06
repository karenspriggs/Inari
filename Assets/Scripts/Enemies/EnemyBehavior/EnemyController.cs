using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Inactive,
    Confused,
    Wander,
    ChaseStartup,
    Chase,
    Attack,
    Hit,
    Stun,
    Launched,
    DeadLaunch,
    Death
}

public class EnemyController : MonoBehaviour
{
    EnemyAnimator enemyAnimator;
    EnemyData enemyData;
    EnemySound enemySounds;
    EnemyParticles enemyParticles;
    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;

    public GameObject animationProps;

    [SerializeField]
    EnemyState currentState;

    bool canMove = true;
    bool canAttack = false;
    bool shouldWander = false;
    bool shouldAttack = false;
    bool shouldHitStun = false;
    bool targetChosen = false;
    bool isGrounded = true;

    [SerializeField]
    float attackTimer;
    float wanderTimer;

    [SerializeField]
    Vector2 currentWanderTarget;
    float currentWanderMin;
    float currentWanderMax;
    public float lastWanderDistance;

    public bool isFacingRight;

    public float AttackDistance;
    public float ChaseSpeed;
    public float WanderSpeed;
    public float xChaseDistance;
    public float yChaseDistance;
    public float xStopDistance;
    public float yStopDistance;
    public float attackCooldown;
    public float WanderCooldown;
    public int maxWanderDistance;
    public bool willWander = true;
    public Vector2 deathLaunchDistance;

    private LayerMask groundMask;
    private LayerMask platformMask;

    public float groundCheckXDistance = 0.25f;
    public float groundCheckYDistance = 0.25f;

    bool groundedLastFrame = false;

    public GameObject chaseTarget;

    private Vector2 hitLocation;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
        enemyParticles = GetComponent<EnemyParticles>();
        enemySounds = GetComponent<EnemySound>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();

        currentState = EnemyState.Idle;
        attackTimer = attackCooldown;
        wanderTimer = WanderCooldown;

        groundMask = LayerMask.GetMask("Ground");
        platformMask = LayerMask.GetMask("One-Way Platform");

        if (xStopDistance <= xChaseDistance)
        {
            Debug.LogError($"{gameObject.name} has a stop distance that is shorter than their chase distance. This will cause their chase behavior to not work properly. Make the stop distance larger or I will nuclear strike your house - John Unity");
        }

        hitLocation = Vector2.zero;

        chaseTarget = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (currentState != EnemyState.Death && currentState != EnemyState.DeadLaunch)
        {
            if (currentState != EnemyState.Hit && currentState != EnemyState.ChaseStartup)
            {
                DetermineIfShouldChase();
                DetermineState();
            }

            UpdateTimers();
            DoState(currentState);
        }
        UpdateGrounding();
    }

    public void SwitchState(EnemyState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case (EnemyState.Idle):
                enemyAnimator.SwitchState(EnemyState.Idle);
                wanderTimer = WanderCooldown;
                canMove = true;
                break;
            case (EnemyState.Wander):
                enemyAnimator.SwitchState(EnemyState.Wander);
                DetermineWanderBoundaries();
                DetermineWanderDistance();
                break;
            case (EnemyState.ChaseStartup):
                enemyAnimator.SwitchState(EnemyState.ChaseStartup);
                enemySounds.PlaySound(enemySounds.AlertSound);
                attackTimer = attackCooldown;
                break;
            case (EnemyState.Confused):
                enemyAnimator.SwitchState(EnemyState.Confused);
                enemySounds.PlaySound(enemySounds.ConfusedSound);
                break;
            case (EnemyState.Chase):
                enemyAnimator.SwitchState(EnemyState.Chase);
                break;
            case (EnemyState.Attack):
                enemyAnimator.SwitchState(EnemyState.Attack);
                enemySounds.PlaySound(enemySounds.AttackSound);
                break;
            case (EnemyState.Hit):
                enemyParticles.PlayHitParticles();
                enemySounds.PlaySound(enemySounds.HitSound);
                enemyAnimator.SwitchState(EnemyState.Hit);
                break;
            case (EnemyState.Stun):
                break;
            case (EnemyState.DeadLaunch):
                enemyAnimator.SwitchState(EnemyState.DeadLaunch);
                break;
            case (EnemyState.Death):
                enemyAnimator.SwitchState(EnemyState.Death);
                enemySounds.PlaySound(enemySounds.DeathSound);
                break;
        }
    }

    public void DoState(EnemyState state)
    {
        switch (state)
        {
            case (EnemyState.Idle):
                canMove = true;
                break;
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.ChaseStartup):
                AnimationEndTransitionToNextState(EnemyState.Chase);
                break;
            case (EnemyState.Confused):
                AnimationEndTransitionToNextState(EnemyState.Idle);
                break;
            case (EnemyState.Chase):
                ChasePlayer();
                break;
            case (EnemyState.Attack):
                canAttack = false;
                attackTimer = attackCooldown;
                AnimationEndTransitionToNextState(EnemyState.Chase);
                break;
            case (EnemyState.Hit):
                canAttack = false;
                attackTimer = attackCooldown;
                AnimationEndTransitionToNextState(EnemyState.Chase);
                break;
            case (EnemyState.Stun):
                break;
            case (EnemyState.Death):
                canAttack = false;
                canMove = false;
                break;
        }
    }

    void DetermineWanderBoundaries()
    {
        currentWanderMin = transform.position.x - maxWanderDistance;
        currentWanderMax = transform.position.x + maxWanderDistance;
    }

    void DetermineIfShouldChase()
    {
        if (currentState != EnemyState.Chase && currentState != EnemyState.Attack)
        {
            float xTargetDistance = Mathf.Abs(transform.position.x - chaseTarget.transform.position.x);
            float yTargetDistance = Mathf.Abs(transform.position.y - chaseTarget.transform.position.y);

            if (xTargetDistance <= xChaseDistance && yTargetDistance <= yChaseDistance)
            {
                Debug.Log("start chasing");
                SwitchState(EnemyState.ChaseStartup);
            }
        }
    }

    void DetermineIfShouldWander()
    {
        if (shouldWander && willWander && currentState == EnemyState.Idle && currentState != EnemyState.Confused)
        {
            SwitchState(EnemyState.Wander);
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
            } else
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
            SwitchState(EnemyState.Hit);
            shouldHitStun = false;
        }
    }

    void TurnAround()
    {
        if (currentState == EnemyState.Wander)
        {
            //Debug.Log("Turning Around");
            if (isFacingRight)
            {
                currentWanderTarget = new Vector2(currentWanderMin, transform.position.y);
            } else
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
            SwitchState(EnemyState.Idle);
        }
    }

    void ChasePlayer()
    {
        Vector3 localScale = transform.localScale;

        if (transform.position.x < chaseTarget.transform.position.x && !isFacingRight)
        {
            FlipEnemy();
        }

        if (transform.position.x > chaseTarget.transform.position.x && isFacingRight)
        {
            FlipEnemy();
        }

        Vector2 currentMovement = Vector2.MoveTowards(transform.position, new Vector2(chaseTarget.transform.position.x, transform.position.y), ChaseSpeed * Time.deltaTime);

        float targetDistance = Vector2.Distance(transform.position, chaseTarget.transform.position);

        float xTargetDistance = Mathf.Abs(transform.position.x - chaseTarget.transform.position.x);
        float yTargetDistance = Mathf.Abs(transform.position.y - chaseTarget.transform.position.y);

        if (targetDistance <= AttackDistance && canAttack)
        {
            SwitchState(EnemyState.Attack);
            return;
        }

        if (xTargetDistance >= xStopDistance || yTargetDistance >= yStopDistance )
        {
            Debug.Log("Out of range");
            SwitchState(EnemyState.Confused);
        }
        else
            transform.position = currentMovement;
    }

    public void LaunchOnDeath(bool isToLeft)
    {
        Debug.Log("Launching enemy");

        Vector2 distanceToLaunch = deathLaunchDistance;

        if (!isToLeft)
        {
            distanceToLaunch = new Vector2(-distanceToLaunch.x, distanceToLaunch.y);
        }

        rigidbody.AddForce(distanceToLaunch);

        SwitchState(EnemyState.DeadLaunch);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void ResetWander()
    {
        shouldWander = true;
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

    private void UpdateTimers()
    {
        UpdateAttackTimer();
        UpdateWanderTimer();
    }

    void UpdateAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer -= 1 * Time.deltaTime;

            if (attackTimer <= 0)
            {
                //just now passed below the timer
                attackTimer = 0f;
                ResetAttack();
            }
        }
    }

    void UpdateWanderTimer()
    {
        if (wanderTimer > 0)
        {
            wanderTimer -= 1 * Time.deltaTime;

            if (wanderTimer <= 0)
            {
                //just now passed below the timer
                wanderTimer = 0f;
                ResetWander();
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
        } else
        {
            if (currentState != EnemyState.Hit)
            {
                rigidbody.gravityScale = 5f;
            }
        }

        if (isGrounded && currentState == EnemyState.DeadLaunch && !groundedLastFrame)
        {
            SwitchState(EnemyState.Death);
            enemyParticles.PlayLandParticles();
        }

        groundedLastFrame = isGrounded;
    }

    private void AnimationEndTransitionToNextState(EnemyState nextState)
    {
        if (enemyAnimator.CheckIfAnimationEnded())
        {
            SwitchState(nextState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox") && (currentState != EnemyState.Death && currentState != EnemyState.DeadLaunch))
        {
            SwitchState(EnemyState.Hit);
            hitLocation = collision.ClosestPoint(new Vector2(transform.position.x, transform.position.y));
        }

        if (collision.gameObject.CompareTag("Bounds"))
        {
            TurnAround();
        }

        if (collision.gameObject.CompareTag("Bumperbox") && currentState == EnemyState.Wander)
        {
            TurnAround();
        }
    }
}

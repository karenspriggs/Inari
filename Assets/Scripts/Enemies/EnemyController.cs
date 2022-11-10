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
    EnemyAnimator enemyAnimatior;
    EnemyData enemyData;
    EnemyParticles enemyParticles;
    [SerializeField]
    EnemyState currentState;

    bool canMove = true;
    bool canAttack = false;
    bool shouldWander = false;
    bool shouldAttack = false;
    bool shouldHitStun = false;
    bool targetChosen = false;

    [SerializeField]
    float attackTimer;
    float wanderTimer;

    [SerializeField]
    Vector2 currentWanderTarget;
    float currentWanderMin;
    float currentWanderMax;

    public bool isFacingRight;

    public float AttackDistance;
    public float ChaseSpeed;
    public float WanderSpeed;
    public float chaseDistance;
    public float stopDistance;
    public float attackCooldown;
    public float WanderCooldown;
    public int maxWanderDistance;
    public bool willWander = true;

    public GameObject chaseTarget;

    private Vector2 hitLocation;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimatior = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
        enemyParticles = GetComponent<EnemyParticles>();
        currentState = EnemyState.Idle;
        attackTimer = attackCooldown;
        wanderTimer = WanderCooldown;

        if (stopDistance <= chaseDistance)
        {
            Debug.LogError($"{gameObject.name} has a stop distance that is shorter than their chase distance. This will cause their chase behavior to not work properly. Make the stop distance larger or I will nuclear strike your house - John Unity");
        }

        hitLocation = Vector2.zero;

        chaseTarget = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (currentState != EnemyState.Death)
        {
            if (currentState != EnemyState.Hit)
            {
                DetermineIfShouldChase();
                DetermineState();
            }
            UpdateTimers();
            DoState(currentState);
        }   
    }

    public void SwitchState(EnemyState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case (EnemyState.Idle):
                enemyAnimatior.SwitchState(EnemyState.Idle);
                canMove = true;
                break;
            case (EnemyState.Wander):
                enemyAnimatior.SwitchState(EnemyState.Wander);
                DetermineWanderDistance();
                break;
            case (EnemyState.ChaseStartup):
                enemyAnimatior.SwitchState(EnemyState.ChaseStartup);
                break;
            case (EnemyState.Chase):
                enemyAnimatior.SwitchState(EnemyState.Chase);
                break;
            case (EnemyState.Attack):
                enemyAnimatior.SwitchState(EnemyState.Attack);
                break;
            case (EnemyState.Hit):
                enemyParticles.PlayHitParticles();
                enemyAnimatior.SwitchState(EnemyState.Hit);
                break;
            case (EnemyState.Stun):
                break;
            case (EnemyState.Death):
                enemyAnimatior.SwitchState(EnemyState.Death);
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
                DetermineWanderBoundaries();
                break;
            case (EnemyState.ChaseStartup):
                AnimationEndTransitionToNextState(EnemyState.Chase);
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
            float targetDistance = Vector2.Distance(transform.position, chaseTarget.transform.position);
            if (targetDistance <= chaseDistance)
            {
                Debug.Log("start chasing");
                SwitchState(EnemyState.Chase);
            }
        }
    }

    void DetermineIfShouldWander()
    {
        if (shouldWander && willWander && currentState == EnemyState.Idle)
        {
            SwitchState(EnemyState.Wander);
        }
    }

    void DetermineWanderDistance()
    {
        if (!targetChosen)
        {
            float wanderDirection = Random.Range(0,2);

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
        if (transform.position.x < currentWanderTarget.x && !isFacingRight)
        {
            FlipEnemy();
        }

        if (transform.position.x > currentWanderTarget.x && isFacingRight)
        {
            FlipEnemy();
        }

        Vector2 currentMovement = Vector2.MoveTowards(transform.position, currentWanderTarget, WanderSpeed * Time.deltaTime);
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

        if (Mathf.Abs(chaseTarget.transform.position.x - transform.position.x) <= AttackDistance && canAttack)
        {
            SwitchState(EnemyState.Attack);
            return;
        }

        if (Vector2.Distance(chaseTarget.transform.position, this.transform.position) >= stopDistance)
        {
            Debug.Log("Out of range");
            SwitchState(EnemyState.Idle);
            wanderTimer = WanderCooldown;
        }
        else 
            transform.position = currentMovement;
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

    private void AnimationEndTransitionToNextState(EnemyState nextState)
    {
        if (enemyAnimatior.CheckIfAnimationEnded())
        {
            SwitchState(nextState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            SwitchState(EnemyState.Hit);
            Debug.Log("hit");
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

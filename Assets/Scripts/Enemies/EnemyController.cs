using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Chase,
    Attack,
    Hit,
    Death
}

public class EnemyController : MonoBehaviour
{
    EnemyAnimator enemyAnimatior;
    EnemyData enemyData;
    [SerializeField]
    EnemyState currentState;

    bool canMove = true;
    bool canAttack = false;
    bool shouldWander = false;
    bool shouldAttack = false;
    bool shouldHitStun = false;
    bool targetChosen = false;
    float attackTimer;
    float wanderTimer;

    [SerializeField]
    Vector2 currentWanderTarget;

    public bool isFacingRight;

    public float ChaseSpeed;
    public float WanderSpeed;
    public float chaseDistance;
    public float stopDistance;
    public float attackCooldown;
    public float WanderCooldown;
    public int maxWanderDistance;

    public GameObject chaseTarget;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimatior = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
        currentState = EnemyState.Idle;
        attackTimer = attackCooldown;
        wanderTimer = WanderCooldown;
    }

    private void FixedUpdate()
    {
        DetermineIfShouldChase();
        UpdateTimers();
        DetermineState();
        DoState(currentState);
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
            case (EnemyState.Chase):
                enemyAnimatior.SwitchState(EnemyState.Chase);
                break;
            case (EnemyState.Attack):
                enemyAnimatior.SwitchState(EnemyState.Attack);
                break;
            case (EnemyState.Hit):
                enemyAnimatior.SwitchState(EnemyState.Hit);
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
                DetermineIfShouldWander();
                break;
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Chase):
                ChasePlayer();
                break;
            case (EnemyState.Attack):
                canAttack = false;
                attackTimer = attackCooldown;
                AnimationEndTransitionToNextState(EnemyState.Idle);
                break;
            case (EnemyState.Hit):
                canAttack = false;
                attackTimer = attackCooldown;
                break;
            case (EnemyState.Death):
                canAttack = false;
                canMove = false;
                break;
        }
    }

    void DetermineIfShouldChase()
    {
        float targetDistance = Vector2.Distance(transform.position, chaseTarget.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            SwitchState(EnemyState.Chase);
        }
    }

    void DetermineIfShouldWander()
    {
        if (shouldWander && currentState == EnemyState.Idle)
        {
            SwitchState(EnemyState.Wander);
        }
    }

    void DetermineWanderDistance()
    {
        if (!targetChosen)
        {
            //switch to a timer lmao
            float currentWanderDistance = Random.Range(-maxWanderDistance, maxWanderDistance);
            currentWanderTarget = new Vector2(currentWanderDistance += transform.position.x, transform.position.y);

            targetChosen = true;
        }
    }

    void DetermineState()
    {
        if (shouldAttack)
        {
            SwitchState(EnemyState.Attack);
            shouldAttack = false;
        }

        if (shouldHitStun)
        {
            SwitchState(EnemyState.Hit);
            shouldHitStun = false;
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
            Debug.Log("Back to idle");
            targetChosen = false;
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
            shouldHitStun = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            shouldAttack = true;
        }
    }
}

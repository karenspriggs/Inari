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
    EnemyState currentState;

    bool canMove = true;
    bool canAttack = false;
    bool shouldAttack = false;
    bool shouldHitStun = false;
    float attackTimer;
    float wanderTimer = 0;
    
    public bool isFacingRight;

    public float speed;
    public float chaseDistance;
    public float stopDistance;
    public float attackCooldown;

    public GameObject chaseTarget;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimatior = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
        currentState = EnemyState.Idle;
        attackTimer = attackCooldown;
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

        attackTimer = 0f;
    }

    public void DoState(EnemyState state)
    {
        switch (state)
        {
            case (EnemyState.Idle):
                canMove = true;
                break;
            case (EnemyState.Wander):
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
        else
            SwitchState(EnemyState.Idle);
    }

    void DetermineState()
    {
        if (shouldAttack)
        {
            SwitchState(EnemyState.Attack);
            shouldAttack = false;
            canAttack = false;
        }

        if (shouldHitStun)
        {
            SwitchState(EnemyState.Hit);
            shouldHitStun = false;
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

        Vector2 currentMovement = Vector2.MoveTowards(transform.position, new Vector2(chaseTarget.transform.position.x, transform.position.y), speed * Time.deltaTime);
        transform.position = currentMovement;
    }

    void ResetAttack()
    { 
        if (!canAttack)
        {
            canAttack = true;
        }
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
    }

    void UpdateAttackTimer()
    {
        if (attackTimer > 0)
        {
            Debug.Log(attackTimer);
            attackTimer -= 1 * Time.deltaTime;
        }

        if (attackTimer <= 0)
        {
            //just now passed below the timer
            attackTimer = 0f;
            ResetAttack();
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

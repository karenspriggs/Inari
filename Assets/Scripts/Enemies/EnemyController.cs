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
    bool canAttack = true;

    public GameObject chaseTarget; 

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimatior = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
    }

    private void FixedUpdate()
    {
        DoState(currentState);
    }

    public void SwitchState(EnemyState newState)
    {
        switch (newState)
        {
            case (EnemyState.Idle):
                enemyAnimatior.SwitchState(EnemyState.Idle);
                canMove = true;
                canAttack = true;
                break;
            case (EnemyState.Wander):
                break;
            case (EnemyState.Chase):
                break;
            case (EnemyState.Attack):
                break;
            case (EnemyState.Hit):
                break;
            case (EnemyState.Death):
                break;
        }
    }

    public void DoState(EnemyState state)
    {
        switch (state)
        {
            case (EnemyState.Idle):

                break;
            case (EnemyState.Wander):
                break;
            case (EnemyState.Chase):
                break;
            case (EnemyState.Attack):
                break;
            case (EnemyState.Hit):
                break;
            case (EnemyState.Death):
                break;
        }
    }
}

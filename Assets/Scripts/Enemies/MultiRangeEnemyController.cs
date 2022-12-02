using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MultiRangeEnemyStates
{
    Idle,
    Wander,
    MeleeAlert,
    AttackDash,
    MeleeAttack,
    BackDash,
    RangedAlert,
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

    public GameObject animationProps;

    [SerializeField]
    EnemyState currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

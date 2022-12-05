using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MultiRangeEnemyState
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

    public GameObject chaseTarget;
    public GameObject animationProps;

    [SerializeField]
    MultiRangeEnemyState currentState;

    [SerializeField]
    float attackTimer;
    float wanderTimer;

    bool canMove = true;
    bool canAttack = false;
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
        attackTimer = attackCooldown;
        wanderTimer = WanderCooldown;

        groundMask = LayerMask.GetMask("Ground");
        platformMask = LayerMask.GetMask("One-Way Platform");
    }
    
    public void SwitchState(MultiRangeEnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case (MultiRangeEnemyState.Idle):
                wanderTimer = WanderCooldown;
                canMove = true;
                break;
            case (MultiRangeEnemyState.Wander):
                break;
        }
    }

    public void DoState(MultiRangeEnemyState state)
    {
        switch (state)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

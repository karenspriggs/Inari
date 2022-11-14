using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class EnemyAttack : MonoBehaviour
{
    //[Header("Attack Parameters")]
    //[SerializeField]
    //private float attackCooldown;
    [SerializeField]
    private float range;
    //[SerializeField]
    //private int damage;

    [Header("Collider Parameters")]
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField]
    private LayerMask playerLayer;

    EnemyAnimator enemyAnimator;
    public float cooldownTime;
    bool canAttack = true;
    bool cooldownTimerStarted = false;

    EnemyData enemyData;

    //References

    private void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
    }

    private void Update()
    {
    //    cooldownTimer += Time.deltaTime;

    //    //Attack only when player in sight?
    //    if (PlayerInSight())
    //    {
    //        if (cooldownTimer >= attackCooldown)
    //        {
    //            cooldownTimer = 0;
    //        }

    //        StartAttack();
    //    }
    }
    /*
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack && !enemyData.isDead)
        {
            Debug.Log("Can attack");
            canAttack = false;
            StartAttack();
            if (!cooldownTimerStarted)
            {
                cooldownTimerStarted = true;
                StartCoroutine(AttackCooldownTimer());
            }
        }
    }

    IEnumerator AttackCooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
        cooldownTimerStarted = false;
    }

    private void StartAttack()
    {
        canAttack = false;
        //enemyAnimator.StartAttackAnimation();
    }
}

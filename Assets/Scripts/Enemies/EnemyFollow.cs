using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    public float chaseDistance;
    public float stopDistance;
    public GameObject target;
    public bool hitStun = false;
    public bool isFacingRight;

    Vector2 currentMovement;
    bool isMoving;
    EnemyAnimator enemyAnimator;
    EnemyData enemyData;

    private float targetDistance;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyData = GetComponent<EnemyData>();
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            if (!enemyData.isDead)
            {
                ChasePlayer();
                isMoving = true;
            }
        }
        else
            StopChasePlayer();

        if (!enemyData.isDead)
        {
            enemyAnimator.UpdateMoveAnimation(isMoving);
        }
    }

    private void StopChasePlayer()
    {
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox") && !enemyData.isDead)
        {
            hitStun = true;
            enemyAnimator.StartHitAnimation();
            StartCoroutine(HitStun());
            StopCoroutine(HitStun());
        }
    }

    private void ChasePlayer()
    {
        Vector3 localScale = transform.localScale;

        if (hitStun == false)
        {
            if (transform.position.x < target.transform.position.x && !isFacingRight)
            {
                FlipEnemy();
            }
            
            if (transform.position.x > target.transform.position.x && isFacingRight)
            {
                FlipEnemy();
            }

            currentMovement = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.position = currentMovement;
        }
    }

    private void FlipEnemy()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private IEnumerator HitStun()
    {
        if (hitStun == true)
        {
            yield return new WaitForSeconds(0.2f);
            hitStun = false;
        }
        yield return null;
    }
}

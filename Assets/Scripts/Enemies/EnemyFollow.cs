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

    Vector2 currentMovement;
    float currentSpeed;
    //EnemyAnimator enemyAnimator;

    private float targetDistance;
    // Start is called before the first frame update
    void Start()
    {
        //enemyAnimator = GetComponent<EnemyAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
            ChasePlayer();
        else
            StopChasePlayer();

        //enemyAnimator.UpdateMoveAnimation(currentSpeed);
    }

    private void StopChasePlayer()
    {
        currentSpeed = 0;
        //Do Nothing
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            hitStun = true;
            //enemyAnimator.StartHitAnimation();
            StartCoroutine(HitStun());
            StopCoroutine(HitStun());
        }
    }

    private void ChasePlayer()
    {
        if (hitStun == false)
        {
            if (transform.position.x < target.transform.position.x)
            {
                //GetComponent<SpriteRenderer>().flipX = true;
            }
            else
                //GetComponent<SpriteRenderer>().flipX = false;

            currentMovement = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            currentSpeed = Mathf.Abs(currentMovement.x);
            transform.position = currentMovement;
        }
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

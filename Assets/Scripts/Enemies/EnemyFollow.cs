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

    private float targetDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)
            ChasePlayer();
        else
            StopChasePlayer();
    }

    private void StopChasePlayer()
    {
        //Do Nothing
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            //Hitstun
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x < target.transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
            GetComponent<SpriteRenderer>().flipX = false;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}

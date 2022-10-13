using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class BakenekoAttacks : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float range;
    [SerializeField]
    private int damage;

    [Header("Collider Parameters")]
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField]
    private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
            }
        }
    }

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

    private void DamagePlayer()
    {
     //   if (PlayerInSight())
      //     PlayerData.TakeDamage(1f);
      //   Debug.Log("Player attacked");
    }
}

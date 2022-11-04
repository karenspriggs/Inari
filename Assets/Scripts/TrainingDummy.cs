using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    public float HP;

    void CheckIfDead()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            HP -= collision.GetComponentInParent<PlayerData>().Attack;
            CheckIfDead();
        }
    }
}

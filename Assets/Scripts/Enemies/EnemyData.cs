using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int currentHP;
    public int Attack;
    [SerializeField]
    private int EnemyKillCoins;
    [SerializeField]
    private int EnemyKillEnergy;
    [SerializeField]
    public int EnemyKillXP;

    EnemyAnimator enemyAnimator;
    EnemyController enemyController;

    public static Action EnemyHit;
    public static Action EnemyKilled;
    public static Action<int, int> EnemyKilledValues;

    public bool isNewController = true;
    public bool isDead = false;
    public bool wasHit = false;
    bool wasHitToLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (wasHit && !isDead)
        {
            Debug.Log("Enemy took damage");
            currentHP--;
            EnemyHit?.Invoke();
            wasHit = false;
            CheckIfDead();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if (currentHP <= 0 && !isDead)
        {
            Debug.Log("Enemy died");
            enemyController.LaunchOnDeath(wasHitToLeft);
            isDead = true;
            EnemyKilled?.Invoke();
            EnemyKilledValues?.Invoke(EnemyKillCoins, EnemyKillXP);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead && collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponentInChildren<CapsuleCollider2D>(), this.gameObject.GetComponent<CapsuleCollider2D>());
            Physics2D.IgnoreCollision(collision.gameObject.GetComponentInChildren<CapsuleCollider2D>(), this.gameObject.GetComponentInChildren<CapsuleCollider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            wasHit = true;

            if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                wasHitToLeft = true;
            } else
            {
                wasHitToLeft = false;
            }
        }
    }
}

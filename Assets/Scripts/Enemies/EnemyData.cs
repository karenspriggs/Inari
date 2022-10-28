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

    public static Action EnemyKilled;
    public static Action<int, int> EnemyKilledValues;

    public bool isDead = false;
    public bool wasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (wasHit)
        {
            Debug.Log("Enemy took damage");
            currentHP--;
            CheckIfDead();
            wasHit = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }

    void CheckIfDead()
    {
        if (currentHP == 0)
        {
            Debug.Log("Enemy died");
            enemyController.SwitchState(EnemyState.Death);
            isDead = true;
            EnemyKilled?.Invoke();
            EnemyKilledValues?.Invoke(EnemyKillCoins, EnemyKillXP);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            wasHit = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead && collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), this.gameObject.GetComponent<CapsuleCollider2D>());
        }
    }
}

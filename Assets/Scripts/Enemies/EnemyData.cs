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
    [SerializeField]
    private int Attack;
    [SerializeField]
    private int EnemyKillCoins;
    [SerializeField]
    private int EnemyKillEnergy;

    EnemyAnimator enemyAnimator;

    public static Action EnemyKilled;

    private void OnEnable()
    {
        EnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyKilled -= OnEnemyKilled;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        enemyAnimator = GetComponent<EnemyAnimator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox"))
        {
            currentHP--;
            CheckIfDead();
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
            EnemyKilled?.Invoke();
        }
    }

    void OnEnemyKilled()
    {
        enemyAnimator.StartDeathAnimation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Security.Cryptography;

public class PlayerData : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    public float maxHP;
    [SerializeField]
    public float currentHP;
    [SerializeField]
    public float Attack;
    [SerializeField]
    public float maxEnergy;

    public float currentEnergy;
    public float dashEnergyCost;
    public float energyForKillingEnemy;
    public float invulnTime;

    public bool isInvincible = false;
    bool isInvincibleCoroutineRunning = false;

    [SerializeField]
    private Weapon currentWeapon;

    private Transform currentCheckpoint;

    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the max amount for health
    /// </summary>
    public static Action<float> InitializedPlayerHealth;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the amount of damage taken
    /// </summary>
    public static Action<float> PlayerTookDamage;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player regained health, it will also give you the amount of damage healed
    /// </summary>
    public static Action<float> PlayerRegainedHP;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the amount of damage taken
    /// </summary>
    public static Action<float> InitializedPlayerEnergy;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the amount of damage taken
    /// </summary>
    public static Action<float> PlayerUsedEnergy;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player died
    /// </summary>
    public static Action PlayerDied;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        currentEnergy = maxEnergy;
        InitializedPlayerHealth?.Invoke(currentHP);
        InitializedPlayerEnergy?.Invoke(currentEnergy);
    }

    private void OnEnable()
    {
        EnemyData.EnemyKilled += OnEnemyKilled;
        GameOverUI.PlayerRestarted += RestartAtCheckPoint;
    }

    private void OnDisable()
    {
        EnemyData.EnemyKilled -= OnEnemyKilled;
        GameOverUI.PlayerRestarted -= RestartAtCheckPoint;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawn()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        PlayerTookDamage?.Invoke(damage);
        CheckIfDead();
    }

    public void HealHealth(int healing)
    {
        float healthGained = healing;

        if (currentHP + healing >= maxHP)
        {
            currentHP = maxHP;
            healthGained = currentHP + healing - maxHP;
        }
        else
        {
            currentHP += healing;
        }

        PlayerRegainedHP?.Invoke(-healthGained);
    }

    public bool TryDashing()
    {
        if (currentEnergy - dashEnergyCost < 0)
        {
            return false;
        }

        return true;
    }

    public void UseEnergy(float energycost)
    {
        currentEnergy -= energycost;
        PlayerUsedEnergy?.Invoke(energycost);
    }

    void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            PlayerDied?.Invoke();
        }
    }

    void OnEnemyKilled()
    {
        float energyGained = energyForKillingEnemy;
        if (maxEnergy < currentEnergy + energyForKillingEnemy)
        {
            energyGained = maxEnergy - currentEnergy;
        }

        currentEnergy += energyGained;
        PlayerUsedEnergy?.Invoke(-energyGained);
    }

    IEnumerator InvulnCoroutine()
    {
        yield return new WaitForSeconds(invulnTime);
        isInvincible = false;
        isInvincibleCoroutineRunning = false;
    }

    public void RestartAtCheckPoint()
    {
        transform.position = currentCheckpoint.position;
        currentHP = maxHP;
        float energyToRecover = maxEnergy - currentEnergy;
        currentEnergy += energyToRecover;
        PlayerTookDamage?.Invoke(-maxHP);
        PlayerUsedEnergy?.Invoke(-energyToRecover);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttackHitbox")
        {
            if (!isInvincible)
            {
                int damage = collision.GetComponentInParent<EnemyData>().Attack;
                TakeDamage(damage);

                if (!isInvincibleCoroutineRunning)
                {
                    isInvincibleCoroutineRunning = true;
                    isInvincible = true;
                    StartCoroutine(InvulnCoroutine());
                }
            }
        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false; //Deactivates checkpoint collider
        }

        if (collision.gameObject.tag == "Food")
        {
            HealHealth(1);
        }
    }
}

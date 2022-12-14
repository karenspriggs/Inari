using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

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
    public float maxHPCap;
    public float maxEnergyCap;
    public float attackCap;

    public PlayerStats playerStats;
    public CheckpointManager checkpointManager;
    public PlayerController playerController;

    public float currentEnergy;
    public float dashEnergyCost;
    public float energyForKillingEnemy;
    public float invulnTime;
    public int currentCheckpointID;
    public int manualCheckpointSpawnID = 0;

    public bool isUsingSaveData = false;
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
        if (checkpointManager.HasCheckpoints())
        {
            if (isUsingSaveData)
            {
                currentCheckpointID = PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID;
                transform.position = checkpointManager.ReturnCheckpointTransform(currentCheckpointID);

                maxHP = PlayerSaveSystem.SessionSaveData.playerStats.MaxHP;
                maxEnergy = PlayerSaveSystem.SessionSaveData.playerStats.MaxEnergy;
                Attack = PlayerSaveSystem.SessionSaveData.playerStats.Attack;
            }
            else
            {
                transform.position = checkpointManager.ReturnCheckpointTransform(manualCheckpointSpawnID);
            }
        }

        PlayerSaveSystem.SessionSaveData.currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        playerController = GetComponent<PlayerController>();

        currentHP = maxHP;
        currentEnergy = maxEnergy;
        InitializedPlayerHealth?.Invoke(currentHP);
        InitializedPlayerEnergy?.Invoke(currentEnergy);
    }

    private void OnEnable()
    {
        EnemyData.EnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyData.EnemyKilled -= OnEnemyKilled;
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
            PlayerSaveSystem.SessionSaveData.playerStats.TimesDiedInLevel++;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttackHitbox")
        {
            if (!isInvincible && playerController.currentState != InariState.Dashing)
            {
                int damage;

                // i hate this lmao but i dont want to make an interface right now UWAA
                if (collision.GetComponentInParent<EnemyData>() == null)
                {
                    damage = collision.gameObject.GetComponent<Projectile>().projectileDamage;
                } else
                {
                    damage = collision.GetComponentInParent<EnemyData>().Attack;
                }

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
    }
}

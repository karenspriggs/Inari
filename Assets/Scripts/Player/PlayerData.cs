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
    private float maxHP;
    [SerializeField]
    private float currentHP;
    [SerializeField]
    private float Attack;
    [SerializeField]
    private float maxEnergy;
    [SerializeField]
    private Weapon currentWeapon;
    public float currentEnergy;
    private Transform currentCheckpoint;

    public bool usingCheckpoints = true;

    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the amount of damage taken
    /// </summary>
    public static Action<float> InitializedPlayerHealth;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the amount of damage taken
    /// </summary>
    public static Action<float> PlayerTookDamage;
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

    void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            PlayerDied?.Invoke();

            if (usingCheckpoints)
            {
                transform.position = currentCheckpoint.position;
                currentHP = 2;
            }
        }
        // If they are alive what do
        if (currentHP >= 0)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttackHitbox")
        {
            TakeDamage(1f);
        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false; //Deactivates checkpoint collider
        }
    }
}

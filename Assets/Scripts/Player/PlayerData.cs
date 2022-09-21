using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerData : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int currentHP;
    [SerializeField]
    private int Attack;
    [SerializeField]
    private Weapon currentWeapon;

    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player took damage, it will also give you the amount of damage taken
    /// </summary>
    public static Action<int> PlayerTookDamage;
    /// <summary>
    /// Subscribe to this delegate for methods that want to know when a player died
    /// </summary>
    public static Action PlayerDied;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        currentHP -= damage;
        PlayerTookDamage?.Invoke(damage);
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if (currentHP >= 0)
        {
            PlayerDied?.Invoke();
        }
    }
}

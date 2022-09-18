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

    public static Action<int> PlayerTookDamage; 

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        TakeDamage(1);
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
    }
}

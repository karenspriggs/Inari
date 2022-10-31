using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public float MaxHP;
    public float CurrentHP;
    public float Attack;
    public float MaxEnergy;
    public float CurrencyCount;
    public float CurrentXP;
    public int CurrentLevel;
    public int LatestCheckpointID;

    public PlayerStats()
    {
        MaxHP = 6;
        CurrentHP = MaxHP;
        Attack = 1;
        MaxEnergy = 20;
        CurrencyCount = 0;
        CurrentXP = 0;
        CurrentLevel = 1;
        LatestCheckpointID = 0;
    }
}

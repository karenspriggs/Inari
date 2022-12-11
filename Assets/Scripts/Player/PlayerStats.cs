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
    public int UpdgradePoints;
    public int LatestCheckpointID;
    public int HighestComboCount;
    public int TimesDiedInLevel;
    public int EnemiesKilled;

    public PlayerStats()
    {
        MaxHP = 6;
        CurrentHP = MaxHP;
        Attack = 1;
        MaxEnergy = 20;
        CurrencyCount = 0;
        CurrentXP = 0;
        CurrentLevel = 1;
        UpdgradePoints = 0;
        LatestCheckpointID = 0;
        HighestComboCount = 0;
        TimesDiedInLevel = 0;
        EnemiesKilled = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerSaveData
{
    public PlayerStats playerStats;
    public int currentLevelIndex;

    public PlayerSaveData()
    {
        playerStats = new PlayerStats();
        currentLevelIndex = 4;
    }
}

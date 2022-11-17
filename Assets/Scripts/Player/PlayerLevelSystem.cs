using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerLevelSystem : MonoBehaviour
{
    public int level;
    public int upgradePoints;

    public float currentXP;
    public float requiredXP;

    private float lerpTimer;
    private float delayTimer;
    public TextMeshProUGUI experienceText;
    public Image frontXpBar;
    public Image backXpBar;
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;
    public float statGain = 1.25f;
    [SerializeField]
    LevelUpScreen levelScreen;

    public bool isNewVersion = false;
    public bool isUsingSaveData = false;

    public static Action<float, float> xpAmountInitialized;
    public static Action<float> PlayerGainedXP;
    public static Action PlayerLeveledUp;

    void Start()
    {
        if (isUsingSaveData)
        {
            this.level = PlayerSaveSystem.SessionSaveData.playerStats.CurrentLevel;
            this.currentXP = PlayerSaveSystem.SessionSaveData.playerStats.CurrentXP;
            this.upgradePoints = PlayerSaveSystem.SessionSaveData.playerStats.UpdgradePoints;
        }

        requiredXP = CalculateRequiredXp();

        if (!isNewVersion)
        {
            frontXpBar.fillAmount = currentXP / requiredXP;
            backXpBar.fillAmount = currentXP / requiredXP;
        }
        
        xpAmountInitialized?.Invoke(currentXP, requiredXP);
    }

    private void OnEnable()
    {
        EnemyData.EnemyKilledValues += GainXPOnEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyData.EnemyKilledValues -= GainXPOnEnemyDeath;
    }

    void CheckIfCanLevelUp()
    {
        if (currentXP > requiredXP)
            LevelUp();
    }

    void GainXPOnEnemyDeath(int coins, int XP)
    {
        this.currentXP += XP;

        if (!isNewVersion)
        {
            UpdateXpUI();
        }
        PlayerGainedXP?.Invoke(currentXP);

        if (isUsingSaveData)
        {
            PlayerSaveSystem.SessionSaveData.playerStats.CurrentXP = currentXP;
        }
        
        CheckIfCanLevelUp();
    }

    public void UpdateXpUI()
    {
        if (!isNewVersion)
        {
            float xpFraction = currentXP / requiredXP;
            float FXP = frontXpBar.fillAmount = xpFraction;
            experienceText.text = currentXP.ToString() + " / " + requiredXP.ToString();
        }
          
        //if (FXP < xpFraction)
        //{
        //    delayTimer += Time.deltaTime;
        //    backXpBar.fillAmount = xpFraction;
        //    if (delayTimer > 3)
        //    {
        //        lerpTimer += Time.deltaTime;
        //        backXpBar.fillAmount = xpFraction;
        //        if (delayTimer > 3)
        //        {
        //            lerpTimer += Time.deltaTime;
        //            float percentComplete = lerpTimer / 4;
        //            frontXpBar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);
        //        }
        //    }
        //}

        //UnityEngine.Debug.Log(xpFraction);
        //UnityEngine.Debug.Log(FXP);
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        currentXP += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
        PlayerGainedXP?.Invoke(currentXP);
    }

    public void GainedExperience()
    {
      
    }

    private void LevelUp()
    {
        if (!isNewVersion)
        {
            frontXpBar.fillAmount = 0f;
            backXpBar.fillAmount = 0f;
        }
        level++;
        upgradePoints++;

        if (isUsingSaveData)
        {
            PlayerSaveSystem.SessionSaveData.playerStats.CurrentLevel = level;
            PlayerSaveSystem.SessionSaveData.playerStats.UpdgradePoints = upgradePoints;
        }

        currentXP = Mathf.RoundToInt(currentXP - requiredXP);
        requiredXP = CalculateRequiredXp();

        xpAmountInitialized?.Invoke(currentXP, requiredXP);
        PlayerGainedXP?.Invoke(currentXP);
        PlayerLeveledUp?.Invoke();
    }

    private int CalculateRequiredXp()
    {

        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;

        
    }
}

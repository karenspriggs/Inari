using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLevelSystem : MonoBehaviour
{
    public int level;
    public float currentXP;
    public float requiredXP;

    private float lerpTimer;
    private float delayTimer;

    public Image frontXpBar;
    public Image backXpBar;
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;
    public float statGain = 1.25f;

    void Start()
    {
        frontXpBar.fillAmount = currentXP / requiredXP;
        backXpBar.fillAmount = currentXP / requiredXP;
        requiredXP = CalculateRequiredXp();
    }

    void Update()
    {
        UpdateXpUI();
       // GainExperienceFlatRate();
        if (currentXP > requiredXP)
            LevelUp();

    }

    public void UpdateXpUI()
    {
          float xpFraction = currentXP / requiredXP;
           float FXP = frontXpBar.fillAmount = xpFraction;
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

        UnityEngine.Debug.Log(xpFraction);
        UnityEngine.Debug.Log(FXP);
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        currentXP += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }

    public void GainedExperience()
    {
      
    }

    private void LevelUp()
    {
        level++;
        frontXpBar.fillAmount = 0f;
        backXpBar.fillAmount = 0f;
        currentXP = Mathf.RoundToInt(currentXP - requiredXP);
        GetComponent<PlayerData>().Attack = Mathf.Ceil(1.25f * GetComponent<PlayerData>().Attack);
        GetComponent<PlayerData>().maxEnergy = Mathf.Ceil(1.25f * GetComponent<PlayerData>().maxEnergy);
        GetComponent<PlayerData>().maxHP = Mathf.Ceil(1.25f * GetComponent<PlayerData>().maxHP);
        requiredXP = CalculateRequiredXp();
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

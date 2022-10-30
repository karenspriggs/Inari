using System.Collections;
using System.Collections.Generic;
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

    [Header("UI")]
    public Image frontXpBar;
    public Image backXpBar;
    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    void Start()
    {
        frontXpBar.fillAmount = currentXP / requiredXP;
        backXpBar.fillAmount = currentXP / requiredXP;
        requiredXP = CalculateRequiredXp();
    }

    void Update()
    {
        UpdateXpUI();
        GainExperienceFlatRate(1);
        if (currentXP > requiredXP)
            LevelUp();

    }

    public void UpdateXpUI()
    {
        float xpFraction = currentXP / requiredXP;
        float FXP = frontXpBar.fillAmount;
        if (FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXpBar.fillAmount = xpFraction;
            if (delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                backXpBar.fillAmount = xpFraction;
                if (delayTimer > 3)
                {
                    lerpTimer += Time.deltaTime;
                    float percentComplete = lerpTimer / 4;
                    frontXpBar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);
                }
            }
        }
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        currentXP += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }

    private void LevelUp()
    {
        level++;
        frontXpBar.fillAmount = 0f;
        backXpBar.fillAmount = 0f;
        currentXP = Mathf.RoundToInt(currentXP - requiredXP);
        GetComponent<PlayerData>().Attack += 1;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;


public class LevelUpScreen : MonoBehaviour
{
    public GameObject LevelUpUIPanel;
    public int sceneIndex;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI progressText;

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;

    public TextMeshProUGUI previewAttackText;
    public TextMeshProUGUI previewHealthText;
    public TextMeshProUGUI previewEnergyText;

    public TextMeshProUGUI levelText;

  //  public TextMeshProUGUI experienceText;
    [SerializeField]
    PlayerData playerStats;
    [SerializeField]
    PlayerLevelSystem playerLevel;
    [SerializeField]
    Button AttackButton;
    [SerializeField]
    Button HealthButton;
    [SerializeField]
    Button EnergyButton;

    public void UpdateLevelUpUI()
    {
        Debug.Log("Updated UI");
        attackText.text = "Current Attack: " + playerStats.Attack.ToString();
        healthText.text = "Current Max Health: " + playerStats.maxHP.ToString();
        energyText.text = "Current Max Energy: " + playerStats.maxEnergy.ToString();

        previewAttackText.text = "Upgrade Value: " + Mathf.Ceil(1.25f * playerStats.Attack).ToString();
        previewHealthText.text = "Upgrade Value: " + Mathf.Ceil(1.25f * playerStats.maxHP).ToString();
        previewEnergyText.text = "Upgrade Value: " + Mathf.Ceil(1.25f * playerStats.maxEnergy).ToString();

        levelText.text = "Current Level: " + playerLevel.level.ToString();
        pointsText.text = "Upgrade Points: " + playerLevel.upgradePoints.ToString();
        progressText.text = "XP to Next Level: " + (playerLevel.requiredXP - playerLevel.currentXP).ToString();
    }

    public void CloseScreen()
    {
        LevelUpUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AttackIncrease()
    {
        if (playerLevel.upgradePoints > 0)
        {
            playerStats.Attack = Mathf.Ceil(1.25f * playerStats.Attack);
            playerLevel.upgradePoints--;

            PlayerSaveSystem.SessionSaveData.playerStats.Attack = playerStats.Attack;
            PlayerSaveSystem.SessionSaveData.playerStats.UpdgradePoints--;

            UpdateLevelUpUI();
        }
    }

    public void EnergyIncrease()
    {
        if (playerLevel.upgradePoints > 0)
        {
            playerStats.maxEnergy = Mathf.Ceil(1.25f * playerStats.maxEnergy);
            playerStats.currentEnergy = playerStats.maxEnergy;
            playerLevel.upgradePoints--;

            PlayerSaveSystem.SessionSaveData.playerStats.MaxEnergy = playerStats.maxEnergy;
            PlayerSaveSystem.SessionSaveData.playerStats.UpdgradePoints--;

            UpdateLevelUpUI();
        }
    }


    public void HealthIncrease()
    {
        if (playerLevel.upgradePoints > 0)
        {
            playerStats.maxHP = Mathf.Ceil(1.25f * playerStats.maxHP);
            playerStats.currentHP = playerStats.maxHP;

            playerLevel.upgradePoints--;

            PlayerSaveSystem.SessionSaveData.playerStats.MaxHP = playerStats.maxHP;
            PlayerSaveSystem.SessionSaveData.playerStats.UpdgradePoints--;

            UpdateLevelUpUI();
        }
    }
}

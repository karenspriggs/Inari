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
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI levelText;
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

    public void ShowLevelUpUI()
    {
        AttackButton.interactable = (playerStats.Attack < playerStats.attackCap);
        EnergyButton.interactable = (playerStats.maxEnergy < playerStats.maxEnergyCap);
        HealthButton.interactable = (playerStats.maxHP < playerStats.maxHPCap);
        Time.timeScale = 0f;
        UpdateLevelUpUI();
        LevelUpUIPanel.SetActive(true);
    }

    public void UpdateLevelUpUI()
    {
        attackText.text = "Attack: " + playerStats.Attack.ToString();
        healthText.text = "Health: " + playerStats.maxHP.ToString();
        energyText.text = "Energy: " + playerStats.maxEnergy.ToString();
        levelText.text = "Current Level: " + playerLevel.level.ToString();
    }

    public void CloseScreen()
    {
        LevelUpUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AttackIncrease()
    {
        playerStats.Attack = Mathf.Ceil(1.25f * playerStats.Attack);
        attackText.text = "Attack: " + playerStats.Attack.ToString();
        AttackButton.interactable = false;
        EnergyButton.interactable = false;
        HealthButton.interactable = false;
        
    }

    public void EnergyIncrease()
    {
        playerStats.maxEnergy = Mathf.Ceil(1.25f * playerStats.maxEnergy);
        energyText.text = "Energy: " + playerStats.maxEnergy.ToString();
        AttackButton.interactable = false;
        EnergyButton.interactable = false;
        HealthButton.interactable = false;
    }


    public void HealthIncrease()
    {
        playerStats.maxHP = Mathf.Ceil(1.25f * playerStats.maxHP);
        healthText.text = "Health: " + playerStats.maxHP.ToString();
        AttackButton.interactable = false;
        EnergyButton.interactable = false;
        HealthButton.interactable = false;
    }

    public void DisableLevelUpUI()
    {
        LevelUpUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

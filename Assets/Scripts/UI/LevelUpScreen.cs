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

    void ShowLevelUpUI()
    {
        Time.timeScale = 0f;
        LevelUpUIPanel.SetActive(true);
    }

    public void UpdateLevelUpUI()
    {
        attackText.text = "Attack :" + GetComponent<PlayerData>().Attack.ToString();
        healthText.text = "Health :" + GetComponent<PlayerData>().maxHP.ToString();
        energyText.text = "Energy :" + GetComponent<PlayerData>().maxEnergy.ToString();
        levelText.text = "Current Level :" + GetComponent<PlayerLevelSystem>().level.ToString();
    }

    public void DisableLevelUpUI()
    {
        LevelUpUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

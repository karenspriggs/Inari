using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WinScreenUI : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject button;

    public TMP_Text levelNameText;

    public TMP_Text deathCounterText;
    public TMP_Text killCounterText;
    public TMP_Text comboCounterText;

    public string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
            UpdateScreenValues();
            ResetValues();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }

    void UpdateScreenValues()
    {
        levelNameText.text = levelName;
        deathCounterText.text = $"Deaths: {PlayerSaveSystem.SessionSaveData.playerStats.TimesDiedInLevel}";
        killCounterText.text = $"Enemies Killed: {PlayerSaveSystem.SessionSaveData.playerStats.EnemiesKilled}";
        comboCounterText.text = $"Highest Combo Chain: {PlayerSaveSystem.SessionSaveData.playerStats.HighestComboCount}";
    }

    void ResetValues()
    {
        PlayerSaveSystem.SessionSaveData.playerStats.HighestComboCount = 0;
        PlayerSaveSystem.SessionSaveData.playerStats.EnemiesKilled = 0;
        PlayerSaveSystem.SessionSaveData.playerStats.TimesDiedInLevel = 0;
        PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID = 0;
    }
}

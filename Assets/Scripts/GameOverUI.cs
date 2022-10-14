using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject GameOverUIPanel;

    private void OnEnable()
    {
        PlayerData.PlayerDied += ShowGameOverUI;
    }

    private void OnDisable()
    {
        PlayerData.PlayerDied -= ShowGameOverUI;
    }

    void ShowGameOverUI()
    {
        Time.timeScale = 0f;
        GameOverUIPanel.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

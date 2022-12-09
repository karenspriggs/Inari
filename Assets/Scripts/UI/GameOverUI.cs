using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;


public class GameOverUI : MonoBehaviour
{
    public GameObject GameOverUIPanel;
    public static Action PlayerRestarted;
    private int sceneIndex;
    public GameObject gameOverButton;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.PlayerDeathAnimationFinished += ShowGameOverUI;
    }

    private void OnDisable()
    {
        PlayerAnimationEvents.PlayerDeathAnimationFinished -= ShowGameOverUI;
    }

    void ShowGameOverUI()
    {
        Time.timeScale = 0f;
        GameOverUIPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverButton);
    }

    public void ReloadScene()
    {
        Debug.Log("Trying to reload scene");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }
}

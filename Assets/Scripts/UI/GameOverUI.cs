using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameOverUI : MonoBehaviour
{
    public GameObject GameOverUIPanel;
    public static Action PlayerRestarted;

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
    }

    public void ReloadScene()
    {
        PlayerRestarted?.Invoke();
    }
}

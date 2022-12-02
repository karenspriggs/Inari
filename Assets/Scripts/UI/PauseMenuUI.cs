using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject[] pauseObjects;
    public Button[] buttons;
    public int currentSceneIndex;
    bool isPaused = false;

    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            UnPause();
        } else
        {
            Pause();
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        isPaused = false;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
        if (pauseMenuUI != null)
        {
            foreach (GameObject g in pauseObjects)
            {
                g.SetActive(false);
            }

            foreach(Button b in buttons)
            {
                b.interactable = true;
            }

            pauseMenuUI.SetActive(true);
        }
    }

    public void RestartButton()
    {
        PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID = 0;
        UnPause();
        SceneManager.LoadScene(currentSceneIndex);
    }
}

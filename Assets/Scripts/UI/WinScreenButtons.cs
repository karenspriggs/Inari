using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenButtons : MonoBehaviour
{
    public int nextLevelIndex;

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

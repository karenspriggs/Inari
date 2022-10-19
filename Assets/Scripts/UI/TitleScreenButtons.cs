using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtons : MonoBehaviour
{
    public int StartingSceneBuildIndex;
    
    public void StartGame()
    {
        SceneManager.LoadScene(StartingSceneBuildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

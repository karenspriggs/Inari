using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButtons : MonoBehaviour
{
    public int StartingSceneBuildIndex;
    public Button continueButton;

    private void Start()
    {
        continueButton.interactable = PlayerSaveSystem.saveDataExists;
    }

    public void StartGame()
    {
        PlayerSaveSystem.MakeNewGame();
        SceneManager.LoadScene(StartingSceneBuildIndex);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(StartingSceneBuildIndex);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene("OptionsTesting");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

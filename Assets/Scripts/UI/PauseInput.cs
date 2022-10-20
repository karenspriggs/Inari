using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInput : MonoBehaviour
{
    public PauseMenuUI pauseMenu;
    private InputControls pauseInput;
    private InputControls.MenusActions pauseActions;

    private void OnEnable()
    {
        pauseInput = new InputControls();
        pauseActions = pauseInput.Menus;
        pauseActions.Enable();

        pauseActions.Pause.performed += ctx => pauseMenu.TogglePauseMenu();
    }
}

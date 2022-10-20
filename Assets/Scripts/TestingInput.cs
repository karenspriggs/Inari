using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInput : MonoBehaviour
{
    private InputControls testInput;
    private InputControls.DebugActions testActions;

    private bool slowmoToggle;

    public TimeManager timeManager;

    private void OnEnable()
    {
        testInput = new InputControls();
        testActions = testInput.Debug;
        testActions.Enable();

        testActions.SlowmoToggle.performed += ctx => timeManager.ToggleSlowmo();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviors")]
    PlayerMovement playerMovement;

    [Header("Input Settings")]
    public float movementSmoothingSpeed = 1f;

    private InputControls playerInput;
    private InputControls.PlayerActions playerActions;

    private Vector2 inputMovement;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    private void OnEnable()
    {
        playerInput = new InputControls();
        playerActions = playerInput.Player;

        playerActions.Move.performed += OnMovement;
    }

    private void OnDisable()
    {
        playerActions.Move.performed -= OnMovement;
    }

    public void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();

        Debug.Log(inputMovement);

        // Putting the movement and saving it to rawmovement
        rawInputMovement = new Vector3(inputMovement.x, inputMovement.y, 0);
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement()
    {
        // Updating the movement data for movement class
        playerMovement.UpdateMovementData(smoothInputMovement);
    }

    private void Update()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
    }
}

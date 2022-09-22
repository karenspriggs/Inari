using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviors")]
    PlayerMovement playerMovement;
    PlayerAnimator playerAnimator;

    [Header("Input Settings")]
    public float movementSmoothingSpeed = 1f;

    private InputControls playerInput;
    private InputControls.PlayerActions playerActions;

    private Vector2 inputMovement;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    private float isBasicAttacking;

    private void OnEnable()
    {
        playerInput = new InputControls();
        playerActions = playerInput.Player;
        playerActions.Enable();

        playerActions.Move.performed += ctx => rawInputMovement = ctx.ReadValue<Vector2>();
        playerActions.Move.canceled += ctx => rawInputMovement = Vector2.zero;

        playerActions.Attack.performed += ctx => isBasicAttacking = ctx.ReadValue<float>();
        playerActions.Attack.canceled += ctx => isBasicAttacking = ctx.ReadValue<float>();
    }

    private void OnDisable()
    {
        playerActions.Move.performed -= ctx => rawInputMovement = ctx.ReadValue<Vector2>();
        playerActions.Move.canceled -= ctx => rawInputMovement = Vector2.zero;

        playerActions.Attack.performed -= ctx => isBasicAttacking = ctx.ReadValue<float>();
        playerActions.Attack.canceled -= ctx => isBasicAttacking = ctx.ReadValue<float>();
    }

    public void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector2.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
        playerAnimator.UpdateMoveAnimation(smoothInputMovement);
    }

    void UpdatePlayerMovement()
    {
        // Updating the movement data for movement class
        playerMovement.UpdateMovementData(smoothInputMovement);
    }

    void UpdatePlayerBasicAttack()
    {
        playerAnimator.UpdateBaseAttackAnimation(isBasicAttacking);
    }

    private void Update()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerBasicAttack();
    }
}

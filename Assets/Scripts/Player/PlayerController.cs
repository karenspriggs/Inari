using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public enum InariState
{
    Neutral,
    Dashing
}

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

    private bool hasJumped = false;
    private float isJumping;
    private float isDashing;
    private float isBasicAttacking;

    private InariState currentState = InariState.Neutral;

    private void OnEnable()
    {
        playerInput = new InputControls();
        playerActions = playerInput.Player;
        playerActions.Enable();

        playerActions.Move.performed += ctx => rawInputMovement = ctx.ReadValue<Vector2>();
        playerActions.Move.canceled += ctx => rawInputMovement = Vector2.zero;

        playerActions.Jump.performed += ctx => isJumping = ctx.ReadValue<float>();
        playerActions.Jump.canceled += ctx => isJumping = ctx.ReadValue<float>();

        playerActions.Dash.performed += ctx => isDashing = ctx.ReadValue<float>();
        playerActions.Dash.canceled += ctx => isDashing = ctx.ReadValue<float>();

        playerActions.Attack.performed += ctx => isBasicAttacking = ctx.ReadValue<float>();
        playerActions.Attack.canceled += ctx => isBasicAttacking = ctx.ReadValue<float>();
    }

    private void OnDisable()
    {
        playerActions.Move.performed -= ctx => rawInputMovement = ctx.ReadValue<Vector2>();
        playerActions.Move.canceled -= ctx => rawInputMovement = Vector2.zero;

        playerActions.Jump.performed -= ctx => isJumping = ctx.ReadValue<float>();
        playerActions.Jump.canceled -= ctx => isJumping = ctx.ReadValue<float>();

        playerActions.Dash.performed -= ctx => isDashing = ctx.ReadValue<float>();
        playerActions.Dash.canceled -= ctx => isDashing = ctx.ReadValue<float>();

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

    void UpdatePlayerDash()
    {
        bool hasDashInputThisFrame = isDashing > 0.1f;
        
        if (hasDashInputThisFrame)
        {
            if (playerMovement.canDash)
            {
                playerAnimator.UpdateDashAnimation();
            }
            playerMovement.UpdateDash();
        }
    }

    void UpdatePlayerJump()
    {
        bool hasJumpInputThisFrame = isJumping == 1;

        if (hasJumpInputThisFrame && !hasJumped)
        {
            Debug.Log("jump");
            playerMovement.UpdateJump();
            hasJumped = true;
            playerAnimator.UpdateJumpAnimation();
        }

        if (hasJumped == true)
        {
            hasJumped = hasJumpInputThisFrame; 
        }
    }

    void UpdatePlayerBasicAttack()
    {
        playerAnimator.UpdateBaseAttackAnimation(isBasicAttacking);
    }

    private void FixedUpdate()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerJump();
        UpdatePlayerDash();
        UpdatePlayerBasicAttack();
    }
}

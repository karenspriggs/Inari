using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public enum InariState
{
    Neutral,
    Dashing,
    Jumping,
    DoubleJumping,
    Air
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

    private float inputMovement;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement; 

    private bool hasJumped = false;
    private float isJumping;
    private float isDashing;
    private float isBasicAttacking;

    public InariState currentState = InariState.Neutral;

    public bool canMove = true;
    public bool canJump = true;
    public bool canDoubleJump = true;
    public bool canDash = true;
    public bool canWallJump = false;
    public bool dashTimerOn = false;
    public bool isFacingRight = true;
    public bool isGrounded = true;

    // state transition flags
    // set these in SwitchState()
    private bool jumpsEnabled = true;
    private bool dashEnabled = true;
    private bool attacksEnabled = true;

    private void OnEnable()
    {
        playerInput = new InputControls();
        playerActions = playerInput.Player;
        playerActions.Enable();

        playerActions.Move.performed += ctx => inputMovement = ctx.ReadValue<float>();

        playerActions.Jump.performed += ctx => isJumping = ctx.ReadValue<float>();
        playerActions.Jump.canceled += ctx => isJumping = ctx.ReadValue<float>();

        playerActions.Dash.performed += ctx => isDashing = ctx.ReadValue<float>();
        playerActions.Dash.canceled += ctx => isDashing = ctx.ReadValue<float>();

        playerActions.Attack.performed += ctx => isBasicAttacking = ctx.ReadValue<float>();
        playerActions.Attack.canceled += ctx => isBasicAttacking = ctx.ReadValue<float>();
    }

    private void OnDisable()
    {
        playerActions.Move.performed += ctx => inputMovement = ctx.ReadValue<float>();

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

    
    void SetMovementInput()
    {
        playerMovement.UpdateMovementData(inputMovement);
    }

    void PlayerRun()
    {
        playerMovement.MoveHorizontal(playerMovement.MaxRunSpeed);
    }

    private bool CheckForDash()
    {
        bool hasDashInputThisFrame = isDashing > 0.1f;
        if (hasDashInputThisFrame)
        {
            if (canDash)
            {
                SwitchState(InariState.Dashing);
                return true;
            }
            
        }
        return false;
    }


    private bool CheckForJump()
    {
        
        bool hasJumpInputThisFrame = isJumping == 1;
        bool r = false;

        if (jumpsEnabled)
        {
            if (hasJumpInputThisFrame && !hasJumped) //jump pressed this frame
            {
                if (canJump)
                {
                    SwitchState(InariState.Jumping);
                    canJump = false;
                    isGrounded = false;
                    r = true;
                }
                else
                {
                    if (canDoubleJump)
                    {
                        SwitchState(InariState.DoubleJumping);
                        canDoubleJump = false;
                        isGrounded = false;
                        r = true;
                    }

                }
                
                hasJumped = true;
            }
            if (hasJumped == true)
            {
                hasJumped = hasJumpInputThisFrame;
            }
        }
        return r;
    }

    void UpdatePlayerBasicAttack()
    {
        playerAnimator.UpdateBaseAttackAnimation(isBasicAttacking);
    }

    private void FixedUpdate()
    {
        
        CheckForInputs();
        
        DoState(currentState);

    }

    public void SwitchState(InariState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case InariState.Neutral:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                break;
            case InariState.Dashing:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                playerMovement.HaltAirVelocity();
                playerAnimator.UpdateDashAnimation();
                playerMovement.DoTheDash();
                break;
            case InariState.Jumping:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                playerMovement.DoTheJump();
                playerAnimator.UpdateJumpAnimation();
                SwitchState(InariState.Air);
                break;
            case InariState.DoubleJumping:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                playerMovement.DoTheJump();
                playerAnimator.UpdateJumpAnimation();
                SwitchState(InariState.Air);
                break;
            case InariState.Air:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                break;
            default:
                break;
        }
    }
    private void DoState(InariState state)
    {
        switch(state)
        {
            case InariState.Neutral:
                playerMovement.UpdateGravity();
                AllowHorizontalMovement();
                break;
            case InariState.Dashing:
                playerMovement.AirPause();
                playerMovement.DoDashFriction();
                if (playerMovement.ShouldEndDash())
                {
                    ReturnToNeutral();
                }
                break;
            case InariState.Jumping:
                playerMovement.UpdateGravity();
                AllowHorizontalMovement();
                break;
            case InariState.DoubleJumping:
                playerMovement.UpdateGravity();
                AllowHorizontalMovement();
                break;
            case InariState.Air:
                playerMovement.UpdateGravity();
                AllowHorizontalMovement();
                break;
        }
    }

    private void CheckForInputs()
    {
        if (CheckForDash()) return;
        if (CheckForJump()) return;
    }

    private void AllowHorizontalMovement()
    {
        SetMovementInput();
        PlayerRun();
        playerMovement.FaceVelocityDir();
    }

    private void ReturnToNeutral()
    {
        if (isGrounded)
        {
            SwitchState(InariState.Neutral);
        }
        else
        {
            SwitchState(InariState.Air);
        }
    }
}

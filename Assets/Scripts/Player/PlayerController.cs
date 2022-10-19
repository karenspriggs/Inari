using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public enum InariState
{
    Neutral,
    DashStartup,
    Dashing,
    Jumping,
    DoubleJumping,
    WallJumping,
    WallSliding,
    Air,
    BasicAttacking,
    Hit,
    Dead
}

public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviors")]
    PlayerMovement playerMovement;
    PlayerAnimator playerAnimator;
    PlayerData playerData;
    PlayerAttacks playerAttacks;

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
    public float DashStartupTimerMax = 0.5f;
    private float stateTimer = 0f; //state timer counts up

    public bool canMove = true;
    public bool canJump = true;
    public bool canDoubleJump = true;
    public bool canDash = true;
    public bool canWallJump = false;
    public bool dashTimerOn = false;
    public bool isFacingRight = true;
    public bool isGrounded = true;
    public bool nextToWall = false;
    bool hasEnabledEnemyCollision = false;
    //public bool canAttack = true;

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

        PlayerData.PlayerTookDamage += SetHit;
        PlayerData.PlayerDied += SetDead;
        GameOverUI.PlayerRestarted += ResetInari;

        PlayerAnimationEvents.PlayerAttackActiveStarted += OnAttackActive;
        PlayerAnimationEvents.PlayerAttackRecoveryStarted += OnAttackRecovery;
        PlayerAnimationEvents.PlayerAnimationVelocityImpulse += OnVelocityImpulse;
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

        PlayerData.PlayerTookDamage -= SetHit;
        PlayerData.PlayerDied -= SetDead;
        GameOverUI.PlayerRestarted -= ResetInari;

        PlayerAnimationEvents.PlayerAttackActiveStarted -= OnAttackActive;
        PlayerAnimationEvents.PlayerAttackRecoveryStarted -= OnAttackRecovery;
        PlayerAnimationEvents.PlayerAnimationVelocityImpulse -= OnVelocityImpulse;
    }

    public void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerData = GetComponent<PlayerData>();
        playerAttacks = GetComponent<PlayerAttacks>();
    }

    void SetMovementInput()
    {
        playerMovement.UpdateMovementData(inputMovement);
        playerAnimator.AnimationUpdateMoveBool(inputMovement);
    }

    void PlayerRun()
    {
        playerMovement.MoveHorizontal(playerMovement.MaxRunSpeed);
    }

    private bool CheckForDash()
    {
        if (dashEnabled)
        {
            bool hasDashInputThisFrame = isDashing > 0.1f;
            if (hasDashInputThisFrame)
            {
                if (playerData.TryDashing())
                {
                    if (canDash && currentState != InariState.DashStartup)
                    {
                        playerData.UseEnergy(playerData.dashEnergyCost);
                        SwitchState(InariState.DashStartup);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool CheckForJump()
    {
        
        bool hasJumpInputThisFrame = (isJumping == 1);
        bool r = false;

        if (jumpsEnabled)
        {
            if (hasJumpInputThisFrame && !hasJumped) //jump pressed this frame
            {
                if (canWallJump && nextToWall && !isGrounded)
                {
                    canWallJump = false;
                    canDoubleJump = true; // give back double jump on wall jump
                    isGrounded = false;
                    r = true;
                    SwitchState(InariState.WallJumping);
                }
                else if (canJump)
                {
                    canJump = false;
                    isGrounded = false;
                    r = true;
                    SwitchState(InariState.Jumping);
                }
                else if (canDoubleJump)
                {
                    canDoubleJump = false;
                    isGrounded = false;
                    r = true;
                    SwitchState(InariState.DoubleJumping);
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
    private bool CheckForBasicAttack()
    {
        if (attacksEnabled)
        {
            bool hasAttackInputThisFrame = isBasicAttacking > 0.1f;
            if (hasAttackInputThisFrame)
            {
                if (currentState != InariState.BasicAttacking)
                {
                    playerAttacks.basicAttacksIndex = 0;
                    SwitchState(InariState.BasicAttacking);
                    return true;
                }
                else
                {
                    //was already attacking, see if we can combo
                    if (playerAttacks.CanBasicAttackCombo())
                    {
                        playerAttacks.basicAttacksIndex += 1;
                        SwitchState(InariState.BasicAttacking);
                    }
                }

            }
        }
        return false;
    }

    private void SetDead()
    {
        SwitchState(InariState.Dead);
    }

    private void SetHit(float hitDamage)
    {
        SwitchState(InariState.Hit);
    }

    private void FixedUpdate()
    {
        CheckForInputs();

        UpdateTimers();

        playerMovement.CheckForGroundedness();

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
                playerAnimator.SwitchState(newState);
                break;
            case InariState.DashStartup:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerMovement.HaltAirVelocity();
                playerAnimator.SwitchState(newState);
                break;
            case InariState.Dashing:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerMovement.HaltAirVelocity();
                playerMovement.DoTheDash();
                break;
            case InariState.Jumping:
            case InariState.DoubleJumping:
            case InariState.WallJumping:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                playerMovement.DoTheJump(newState);
                playerAnimator.SwitchState(newState);
                SwitchState(InariState.Air);
                break;
            case InariState.Air:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                canJump = false;
                canWallJump = true;
                break;
            case InariState.BasicAttacking:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerMovement.HaltAirVelocity();
                playerAnimator.StartAnimation(playerAttacks.basicAttacks[playerAttacks.basicAttacksIndex].Name);
                break;
            case InariState.Hit:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerData.isInvincible = true;
                playerMovement.HaltAirVelocity();
                playerAnimator.SwitchState(newState);
                break;
            case InariState.Dead:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerData.isInvincible = true;
                playerMovement.HaltAirVelocity();
                playerAnimator.SwitchState(newState);
                break;
            default:
                break;
        }
        
        stateTimer = 0f;
    }
    private void DoState(InariState state)
    {
        switch(state)
        {
            case InariState.Neutral:
                playerMovement.UpdateGravity();
                AllowHorizontalMovement();
                AllowFalling();
                AllowGroundToResetJumps();
                EnableEnemyCollision();
                break;
            case InariState.DashStartup:
                playerMovement.SetGravity(playerMovement.DashStartupGravityScale);
                playerMovement.DoFriction(playerMovement.GroundFriction);
                TimeTransitionToNextState(DashStartupTimerMax, InariState.Dashing);
                DisableEnemyCollision();
                break;
            case InariState.Dashing:
                playerMovement.TurnOffGravity();
                playerMovement.DoFriction(playerMovement.DashFriction);
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
                playerMovement.CheckForWalledness();
                if (stateTimer > 0.1f)
                {
                    AllowLanding();
                }
                break;
            case InariState.BasicAttacking:
                playerMovement.TurnOffGravity();
                playerMovement.DoFriction(playerMovement.GroundFriction);
                AnimationEndTransitionToNextState(InariState.Neutral);
                break;
            case InariState.Hit:
                AnimationEndTransitionToNextState(InariState.Neutral);
                break;
        }
    }

    private void CheckForInputs()
    {
        SetMovementInput();
        if (CheckForDash()) return;
        if (CheckForJump()) return;
        if (CheckForBasicAttack()) return;
    }

    private void AllowHorizontalMovement()
    {
        PlayerRun();
        playerMovement.FaceVelocityDir();
    }

    private void AllowGroundToResetJumps()
    {
        if (isGrounded)
        {
            canJump = true; // let the state machine handle these
            canDoubleJump = true;
            canWallJump = true;
        }
    }

    private void AllowFalling()
    {
        if (!isGrounded)
        {
            SwitchState(InariState.Air);
        }
    }

    private void AllowLanding()
    {
        if (isGrounded)
        {
            SwitchState(InariState.Neutral);
        }
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

    private void UpdateTimers()
    {
        UpdateStateTimer();
        playerMovement.UpdateTimers();
    }

    private void UpdateStateTimer()
    {
        stateTimer += 1 * Time.deltaTime;
    }

    private bool CheckStateTimer(float stateTimerMax)
    {
        if (stateTimer >= stateTimerMax)
        {
            return true;
        }
        return false;
    }

    private void TimeTransitionToNextState(float stateTimerMax, InariState nextState)
    {
        if (CheckStateTimer(stateTimerMax))
        {
            SwitchState(nextState);
        }
    }

    private void AnimationEndTransitionToNextState(InariState nextState)
    {
        if (playerAnimator.CheckIfAnimationEnded())
        {
            SwitchState(nextState);
        }
    }

    void ResetInari()
    {
        SwitchState(InariState.Neutral);
    }

    void DisableEnemyCollision()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies != null)
        {
            foreach(GameObject g in enemies)
            {
                Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), g.GetComponent<CapsuleCollider2D>(), true);
            }
        }

        hasEnabledEnemyCollision = false;
    }

    void EnableEnemyCollision()
    {
        if (!hasEnabledEnemyCollision)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies != null)
            {
                foreach (GameObject g in enemies)
                {
                    Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), g.GetComponent<CapsuleCollider2D>(), false);
                }
            }
            hasEnabledEnemyCollision = true;
        }
    }

    #region attacks combos

    public void OnAttackActive()
    {
        //Debug.Log("OnAttackActive Received");
    }

    public void OnAttackRecovery()
    {
        //Debug.Log("OnAttackRecovery Received");
        attacksEnabled = true;
    }

    public void OnVelocityImpulse(object sender, AnimationVelocityEventArgs e)
    {
        Vector2 impulse = e.velocity;
        if (!isFacingRight)
        {
            impulse.x = -impulse.x;
        }
        playerMovement.ApplyVelocityImpulse(impulse);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq.Expressions;

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
    GroundBasicAttacking,
    AirBasicAttacking,
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
    PlayerSound playerSound;
    PlayerParticles playerParticles;

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
    private float isDroppingDown;
    private float isBasicAttacking;
    private bool b_isBasicAttacking;

    private bool isHoldingUp;
    private bool b_isHeavyAttacking;

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
    public bool isPlatformGrounded = false;
    public bool nextToWall = false;
    bool hasEnabledEnemyCollision = false;
    bool canDropDown = false;
    //public bool canAttack = true;
    private bool usedAirAttack = false;
    private bool isInRecovery = false;

    // state transition flags
    // set these in SwitchState()
    private bool jumpsEnabled = true;
    private bool dashEnabled = true;
    private bool attacksEnabled = true;
    private bool heavyAttacksEnabled = true;

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

        playerActions.DropDown.performed += ctx => isDroppingDown = ctx.ReadValue<float>();
        playerActions.DropDown.canceled += ctx => isDroppingDown = ctx.ReadValue<float>();

        playerActions.UseItem.started += ctx => UseQuickSlot();

        //playerActions.Attack.performed += ctx => isBasicAttacking = ctx.ReadValue<float>();
        //playerActions.Attack.canceled += ctx => isBasicAttacking = ctx.ReadValue<float>();

        playerActions.Attack.performed += ctx => onBasicAttackPressed();

        playerActions.HeavyAttack.performed += ctx => onHeavyAttackPressed();

        playerActions.UpButton.performed += ctx => isHoldingUp = (ctx.ReadValue<float>() == 1f);
        playerActions.UpButton.canceled += ctx => isHoldingUp = (ctx.ReadValue<float>() == 1f);

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

        playerActions.DropDown.performed -= ctx => isDroppingDown = ctx.ReadValue<float>();
        playerActions.DropDown.canceled -= ctx => isDroppingDown = ctx.ReadValue<float>();

        //playerActions.Attack.performed -= ctx => isBasicAttacking = ctx.ReadValue<float>();
        //playerActions.Attack.canceled -= ctx => isBasicAttacking = ctx.ReadValue<float>();

        playerActions.Attack.performed -= ctx => onBasicAttackPressed();

        playerActions.HeavyAttack.performed -= ctx => onHeavyAttackPressed();

        playerActions.UpButton.performed -= ctx => isHoldingUp = (ctx.ReadValue<float>() == 1f);
        playerActions.UpButton.canceled -= ctx => isHoldingUp = (ctx.ReadValue<float>() == 1f);

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
        playerSound = GetComponent<PlayerSound>();
        playerParticles = GetComponent<PlayerParticles>();
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
                if (playerData.HasEnergyToDash())
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
                else if (canJump || playerMovement.CanCoyoteJump)
                {
                    canJump = false;
                    playerMovement.CanCoyoteJump = false;
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

    private void CheckForDrop()
    {
        canDropDown = (isDroppingDown == 1f); 
    }

    private void onBasicAttackPressed()
    {
        b_isBasicAttacking = true;
    }

    private void onHeavyAttackPressed()
    {
        b_isHeavyAttacking = true;
    }

    private void UseQuickSlot()
    {
        // TODO: some way to check if inari has an item he can eat and return it here. so i dont play the sound if hes got no food!!!!
        playerSound.PlaySound(playerSound.EatSound);
        GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>().UseItem();
    }


    private bool CheckForBasicAttack()
    {
        if (attacksEnabled)
        {
            //by putting this reset bool here, we effectively have an input buffer that saves an attack press
            // until the next time attacks are enabled
            // maybe not the best permanent solution, but it feels good and works for now
            // unless we someday get a real sophisticated input buffer going
            
            bool hasAttackInputThisFrame = b_isBasicAttacking;
            b_isBasicAttacking = false;

            if (hasAttackInputThisFrame)
            {
            
                if (currentState != InariState.GroundBasicAttacking && currentState != InariState.AirBasicAttacking && HasNotUsedAirComboYetIfInAir())
                {
                    playerAttacks.basicAttacksIndex = 0;
                    SwitchAttackStateBasedOnGroundedness();
                    return true;
                }
                else
                {
                    //was already attacking, see if we can combo
                    if (playerAttacks.CanBasicAttackCombo(isGrounded))
                    {
                        playerAttacks.basicAttacksIndex += 1;
                        SwitchAttackStateBasedOnGroundedness();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool CheckForHeavyAttack()
    {
        if (attacksEnabled)
        {
            bool hasAttackInputThisFrame = b_isHeavyAttacking;
            b_isHeavyAttacking = false;

            if (hasAttackInputThisFrame && IsntCancelingHeavyIntoHeavy() && playerData.HasEnergyToHeavyAttack())
            {
            
                if (isGrounded) //only grounded heavys for now
                {
                    playerData.UseEnergy(playerData.heavyAttackEnergyCost);
                    if (!isHoldingUp)
                    {
                        playerAttacks.currentAttack = playerAttacks.otherAttacks[InariOtherAttacks.GroundHeavy];
                        
                    }
                    else
                    {
                        playerAttacks.currentAttack = playerAttacks.otherAttacks[InariOtherAttacks.GroundLaunch];
                    }
                    SwitchState(InariState.GroundBasicAttacking);
                    return true;
                }
            }
        }
        return false;
    }

    private void SwitchAttackStateBasedOnGroundedness()
    {
        if (isGrounded)
        {
            playerAttacks.currentAttack = playerAttacks.groundBasicAttacks[playerAttacks.basicAttacksIndex];
            SwitchState(InariState.GroundBasicAttacking);
        }
        else
        {
            playerAttacks.currentAttack = playerAttacks.airBasicAttacks[playerAttacks.basicAttacksIndex];
            SwitchState(InariState.AirBasicAttacking);
        }
    }
    
    private bool HasNotUsedAirComboYetIfInAir()
    {
        return !usedAirAttack;
    }

    private bool IsntCancelingHeavyIntoHeavy()
    {
        if (!playerAttacks.IsCurrentlyAttacking)
        {
            return true;
        }

        return playerAttacks.CanHeavyAttackCancel();
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
        if (isGrounded) usedAirAttack = false;

        bool animateGroundedness = isGrounded; //|| isPlatformGrounded;
        playerAnimator.AnimationUpdateGroundedBool(animateGroundedness);

        DoState(currentState);

    }

    public void SwitchState(InariState newState)
    {
        SwitchState(newState, string.Empty);
    }
    public void SwitchState(InariState newState, string args)
    {
        bool hasArgs = (args != string.Empty);

        ExitState(currentState); // do any exit stuff, most states dont have anything anyway

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
                playerMovement.HaltVerticalVelocity();
                playerAnimator.SwitchState(newState);
                DisableEnemyCollision();
                break;
            case InariState.Dashing:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerSound.PlaySound(playerSound.DashSound);
                playerMovement.HaltVerticalVelocity();
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
                playerSound.PlaySound(playerSound.JumpSound);
                //playerParticles.CreateDust();
                SwitchState(InariState.Air);
                break;
            case InariState.Air:
                jumpsEnabled = true;
                dashEnabled = true;
                attacksEnabled = true;
                canWallJump = true;
                playerMovement._coyoteTimer = playerMovement.CoyoteTimer;
                break;
            case InariState.GroundBasicAttacking:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                isInRecovery = false;
                playerAttacks.IsCurrentlyAttacking = true;
                playerSound.PlayAttackSound(playerAttacks.currentAttack.Sound);
                playerMovement.HaltVerticalVelocity();
                playerAnimator.StartAnimation(playerAttacks.currentAttack.Name);
                break;
            case InariState.AirBasicAttacking:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                isInRecovery = false;
                usedAirAttack = true;
                playerAttacks.IsCurrentlyAttacking = true;
                playerSound.PlayAttackSound(playerAttacks.currentAttack.Sound);
                playerMovement.HaltVerticalVelocity();
                playerAnimator.StartAnimation(playerAttacks.currentAttack.Name);
                break;
            case InariState.Hit:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerData.isInvincible = true;
                playerSound.PlaySound(playerSound.HitSound);
                playerMovement.HaltVerticalVelocity();
                playerAnimator.SwitchState(newState);
                break;
            case InariState.Dead:
                jumpsEnabled = false;
                dashEnabled = false;
                attacksEnabled = false;
                playerData.isInvincible = true;
                playerSound.PlaySound(playerSound.DeathSound);
                playerMovement.HaltVerticalVelocity();
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
                break;
            case InariState.DashStartup:
                playerMovement.SetGravity(playerMovement.DashStartupGravityScale);
                playerMovement.DoFriction(playerMovement.GroundFriction);
                TimeTransitionToNextState(DashStartupTimerMax, InariState.Dashing);
                break;
            case InariState.Dashing:
                playerMovement.TurnOffGravity();
                playerMovement.HaltVerticalVelocity(); // no move up and down, hopefully stop trimping?
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
            case InariState.GroundBasicAttacking:
                playerMovement.SetGravity(playerMovement.AttackGravityScale);
                playerMovement.DoFriction(playerMovement.GroundFriction);
                if (isInRecovery)
                {
                    AllowFalling();
                }
                AnimationEndTransitionToNextState(InariState.Neutral);
                break;
            case InariState.AirBasicAttacking:
                playerMovement.SetGravity(playerMovement.AttackGravityScale);
                playerMovement.DoFriction(playerMovement.AirFriction);
                if (isInRecovery)
                {
                    AllowLanding();
                }
                AnimationEndTransitionToNextState(InariState.Air);
                break;
            case InariState.Hit:
                AnimationEndTransitionToNextState(InariState.Neutral);
                break;
        }
    }

    public void ExitState(InariState stateBeingLeft)
    {
        switch (stateBeingLeft)
        {
            case InariState.GroundBasicAttacking:
            case InariState.AirBasicAttacking:
                playerAttacks.IsCurrentlyAttacking = false;
                break;
            default:
                break;
        }
    }

    private void CheckForInputs()
    {
        CheckForDrop();
        SetMovementInput();
        if (CheckForDash()) return;
        if (CheckForJump()) return;
        if (CheckForBasicAttack()) return;
        if (CheckForHeavyAttack()) return;
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
        /*
        if (isPlatformGrounded)
        {
            canJump = true; // let the state machine handle these
            canDoubleJump = true;
        }
        */
    }

    private void AllowFalling()
    {
        if (!isGrounded)// && !isPlatformGrounded)
        {
            SwitchState(InariState.Air);
        }
    }

    private void AllowLanding()
    {
        if (isGrounded)// || isPlatformGrounded)
        {
            playerSound.PlaySound(playerSound.LandingSound);
            playerParticles.CreateDust();
            usedAirAttack = false;
            SwitchState(InariState.Neutral);
        }
    }

    private void ReturnToNeutral()
    {
        EnableEnemyCollision();
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
        if (playerAnimator.CheckIfAnimationEnded() && stateTimer > 0.1f)
        {
            SwitchState(nextState);
        }
    }

    public void ResetInari()
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            if (canDropDown)
            {
                canDropDown = false;
                collision.gameObject.GetComponent<OneWayPlatform>().DropDown(GetComponent<CapsuleCollider2D>());
            }
        }
    }

    #region attacks combos

    public void OnAttackActive()
    {
        //Debug.Log("OnAttackActive Received");
        //SetAttackCancels();
    }

    public void OnAttackRecovery()
    {
        
        SetAttackCancels();
        isInRecovery = true;
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

    public void SetAttackCancels()
    {
        attacksEnabled = true;
        
        //update cancels
        dashEnabled = playerAttacks.CanDashCancel();
        jumpsEnabled = playerAttacks.CanJumpCancel();
    }

    #endregion
}

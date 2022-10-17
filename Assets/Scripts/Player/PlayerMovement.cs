using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float HorizontalAccel;
    public float MaxRunSpeed;

    public float DashSpeed;
    public float DashTimer;
    public float DashFriction;

    public float JumpHeight;
    public float DoubleJumpHeight;
    public float WallJumpHeight;
    public float WallJumpSpeedHorizontal;

    public float RisingGravityScale;
    public float FallingGravityScale;
    public float DashStartupGravityScale = 0.3f;

    public float GroundFriction;
    public float AirFriction;

    private LayerMask groundMask;
    public float groundCheckXDistance = 0.25f;
    public float groundCheckYDistance = 0.25f;
    

    private float inputMovement;

    Rigidbody2D playerRigidbody;
    PlayerController playerController;
    CapsuleCollider2D playerCapsule;
    //BoxCollider2D playerCapsule;
    public float _dashTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        playerController = GetComponent<PlayerController>();
        playerCapsule = GetComponent<CapsuleCollider2D>();
        //playerCapsule = GetComponent<BoxCollider2D>();

        groundMask = LayerMask.GetMask("Ground");
        playerController.canDash = true;
    }


    public void MoveHorizontal(float maxSpeed)
    {
        MoveHorizontal(maxSpeed, inputMovement);
    }

    public void MoveHorizontal(float maxSpeed, float _inputMovement)
    {

        float hsp = playerRigidbody.velocity.x;

        float fricAmount = GroundFriction;
        
        if (playerController.isGrounded)
        {
            fricAmount = GroundFriction;
        }
        else
        {
            fricAmount = AirFriction;
        }

        if (_inputMovement != 0)
        {
            // we move!

            float actingHorizontalAccel = HorizontalAccel;

            /*
            if (hsp != 0 && System.Math.Sign(hsp) != System.Math.Sign(_inputMovement))
            {
                //if we are trying to change directions, give extra traction.
                actingHorizontalAccel += fricAmount;
            }
            */

            if (
                (System.Math.Abs(hsp) <= (maxSpeed - (actingHorizontalAccel * System.Math.Abs(_inputMovement) * Time.deltaTime))) || //if you can accelerate
                (System.Math.Sign(_inputMovement) != System.Math.Sign(hsp)) // or if you are trying to change directions
                )
            {
                hsp += actingHorizontalAccel * _inputMovement * Time.deltaTime;
            }
            else
            {
                // cap speed
                // TODO: maybe soft cap speed? like if ur going over your max speed, just slow down over time until you get back to max speed
                //hsp = maxSpeed * System.Math.Sign(hsp);
                hsp = ApplyHorizontalFriction(fricAmount, hsp, maxSpeed * System.Math.Sign(hsp));
            }
        }
        else
        {
            // we are not moving.
            hsp = ApplyHorizontalFriction(fricAmount, hsp);
        }

        playerRigidbody.velocity = new Vector2(hsp, playerRigidbody.velocity.y);
    }

    private float ApplyHorizontalFriction(float fricAmount, float hsp)
    {
        /*
        if (System.Math.Abs(hsp) >= fricAmount*Time.deltaTime)
        {
            hsp -= System.Math.Sign(hsp) * fricAmount * Time.deltaTime;
        }
        else
        {
            hsp = 0f;
        }

        return hsp;
        */

        return ApplyHorizontalFriction(fricAmount, hsp, 0f);

    }
    private float ApplyHorizontalFriction(float fricAmount, float hsp, float goalhsp)
    {
        if (System.Math.Abs(hsp) >= System.Math.Abs(goalhsp) + (fricAmount * Time.deltaTime))
        {
            hsp -= System.Math.Sign(hsp) * fricAmount * Time.deltaTime;
        }
        else
        {
            hsp = goalhsp;
        }

        return hsp;
    }


    public void UpdateMovementData(float newMovementDirection)
    {
        inputMovement = newMovementDirection;
    }
    #region DASHING
    private void Dash()
    {
        float dashSpeedThisFrame = DashSpeed;

        bool dashRight = true;
        if (inputMovement == 0)
        {
            dashRight = playerController.isFacingRight;
        }
        else
        {
            if (inputMovement < 0)
            {
                dashRight = false;
            }
            else
            {
                dashRight = true;
            }
        }

        if (!dashRight)
        {
            dashSpeedThisFrame = -dashSpeedThisFrame;
        }

        playerRigidbody.velocity = new Vector2(dashSpeedThisFrame, playerRigidbody.velocity.y);
        playerController.canDash = false;
    }
    public void DoTheDash()
    {
        Dash();

        if (!playerController.dashTimerOn)
        {
            playerController.dashTimerOn = true;
            //StartCoroutine(DashCoroutine());
            ResetDashTimer();
        }
    }
    public bool ShouldEndDash()
    {
        if (System.Math.Abs(playerRigidbody.velocity.x) <= MaxRunSpeed)
        {
            return true;
        }
        return false;
    }
    public void ResetDash()
    {
        playerController.canDash = true;
        playerController.dashTimerOn = false;

    }

    public void ResetDashTimer()
    {
        _dashTimer = DashTimer;
    }
    #endregion

    #region JUMPING
    private void Jump()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpHeight);
    }

    private void DoubleJump()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, DoubleJumpHeight);
    }

    private void WallJump()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x + (WallJumpSpeedHorizontal * -GetSignFromRightBool(playerController.isFacingRight)), WallJumpHeight);
    }

    public void DoTheJump(InariState jumpType)
    {
        switch (jumpType)
        {
            case InariState.WallJumping:
                WallJump();
                break;
            case InariState.DoubleJumping:
                DoubleJump();
                break;
            case InariState.Jumping:
            default:
                Jump();
                break;
        }
    }
    #endregion

    public void DoFriction(float frictionAmount)
    {
        float hsp = ApplyHorizontalFriction(frictionAmount, playerRigidbody.velocity.x);
        playerRigidbody.velocity = new Vector2(hsp, playerRigidbody.velocity.y);
    }


    public void FaceVelocityDir()
    {
        if (playerRigidbody.velocity.x != 0)
        {
            bool shouldFaceRight = playerRigidbody.velocity.x > 0;
            if (shouldFaceRight != playerController.isFacingRight) FlipPlayer();
        }
    }

    void FlipPlayer()
    {
        // Flip the player
        playerController.isFacingRight = !playerController.isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }


    public void UpdateGravity()
    {
        if (playerRigidbody.velocity.y <= 0f)
        {
            SetGravity(FallingGravityScale);
        } else
        {
            SetGravity(RisingGravityScale);

        }

    }

    public void TurnOffGravity()
    {
        SetGravity(0f);
    }

    public void SetGravity(float gravScale)
    {
        playerRigidbody.gravityScale = gravScale;
    }

    public void HaltAirVelocity()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
    }


    /// <summary>
    /// Updates cooldown timers like dash cooldown, etc. Call in playercontroller update!
    /// </summary>
    public void UpdateTimers()
    {
        UpdateDashTimer();
    }

    public void UpdateDashTimer()
    {
        if (_dashTimer > 0)
        {
            _dashTimer -= 1 * Time.deltaTime;

            if (_dashTimer <= 0)
            {
                //just now passed below the timer
                _dashTimer = 0f;
                ResetDash();
            }
        }
    }

    public bool CheckDashTimer()
    {
        if (_dashTimer > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// sets isGrounded based on if there is something w the ground layer beneath inari's feet
    /// </summary>
    public void CheckForGroundedness()
    {
        playerController.isGrounded = Physics2D.OverlapArea(new Vector2(playerCapsule.bounds.min.x + groundCheckXDistance, playerCapsule.bounds.min.y), new Vector2(playerCapsule.bounds.max.x - groundCheckXDistance, playerCapsule.bounds.min.y - groundCheckYDistance), groundMask);
    }

    /// <summary>
    /// sets isWalled based on if there is something w the ground layer in front of inari
    /// </summary>
    public void CheckForWalledness()
    {
        float wallCheckX;
        if (playerController.isFacingRight)
        {
            wallCheckX = playerCapsule.bounds.max.x;
        }
        else
        {
            wallCheckX = playerCapsule.bounds.min.x;
        }
        playerController.nextToWall = Physics2D.OverlapArea(new Vector2(wallCheckX, playerCapsule.bounds.min.y + groundCheckYDistance), new Vector2(wallCheckX + groundCheckXDistance * GetSignFromRightBool(playerController.isFacingRight), playerCapsule.bounds.max.y - groundCheckYDistance), groundMask);
    }

    public int GetSignFromRightBool(bool b)
    {
        if (b) return 1;
        return -1;
    }
}

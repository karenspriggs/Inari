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
    CapsuleCollider2D playerCollider;
    PlayerController playerController;
    CapsuleCollider2D playerCapsule;

    public float _dashTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerController = GetComponent<PlayerController>();
        playerCapsule = GetComponent<CapsuleCollider2D>();

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

            if (hsp != 0 && System.Math.Sign(hsp) != System.Math.Sign(_inputMovement))
            {
                //if we are trying to change directions, give extra traction.
                actingHorizontalAccel += fricAmount;
            }

            if (System.Math.Abs(hsp) < maxSpeed || System.Math.Sign(_inputMovement) != System.Math.Sign(hsp))
            {
                hsp += actingHorizontalAccel * _inputMovement * Time.deltaTime;
            }
            else
            {
                // cap speed
                // TODO: maybe soft cap speed? like if ur going over your max speed, just slow down over time until you get back to max speed
                hsp = maxSpeed * System.Math.Sign(hsp);
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
        if (System.Math.Abs(hsp) >= fricAmount*Time.deltaTime)
        {
            hsp -= System.Math.Sign(hsp) * fricAmount * Time.deltaTime;
        }
        else
        {
            hsp = 0f;
        }

        return hsp;
    }


    void Jump()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpHeight);
    }

    public void UpdateMovementData(float newMovementDirection)
    {
        inputMovement = newMovementDirection;
    }

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


    public void DoTheJump()
    {
        Jump();
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

    public void DoFriction(float frictionAmount)
    {
        float hsp = ApplyHorizontalFriction(frictionAmount, playerRigidbody.velocity.x);
        playerRigidbody.velocity = new Vector2(hsp, playerRigidbody.velocity.y);
    }

    public bool ShouldEndDash()
    {
        if (System.Math.Abs(playerRigidbody.velocity.x) <= MaxRunSpeed)
        {
            return true;
        }
        return false;
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

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //playerController.canJump = true; // let the state machine handle these
            //playerController.canDoubleJump = true;
            playerController.isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isGrounded = false;
        }
    }
    */

    public void UpdateGravity()
    {
        if (playerRigidbody.velocity.y <= 0f)
        {
            SetGravity(FallingGravityScale);
        } else
        {
            SetGravity(RisingGravityScale);

        }

        /*
        if (playerController.isGrounded)
        {
            SetGravity(1f);
        }
        */
    }

    public void AirPause()
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

    public void ResetDash()
    {
        playerController.canDash = true;
        playerController.dashTimerOn = false;

    }

    public void ResetDashTimer()
    {
        _dashTimer = DashTimer;
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

    }
}

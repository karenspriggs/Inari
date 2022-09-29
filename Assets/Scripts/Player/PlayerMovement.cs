using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MovementSpeed;
    public float DashDistance;
    public float DeccelFactor;
    public float JumpHeight;
    public float DashTimer;
    public float FallingGravityScale;

    private bool canMove = true;
    public bool canJump = true;
    public bool canDoubleJump = true;
    public bool canDash = true;
    private bool canWallJump = false;
    private bool dashTimerOn = false;
    private bool isFacingRight = true;
    private bool isGrounded = true;

    private LayerMask groundMask;

    private Vector2 movementInput;

    Rigidbody2D playerRigidbody;
    CapsuleCollider2D playerCollider;
    PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerController = GetComponent<PlayerController>();

        groundMask = LayerMask.GetMask("Ground");
        canDash = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //UpdateGrouding();
        UpdateGravity();
        movementInput = Vector2.zero;
    }

    void MovePlayer()
    {
        if (!canMove) return;

        Vector2 movementThisFrame = new Vector2(movementInput.x, 0) * MovementSpeed * Time.deltaTime;

        if (!isFacingRight && movementInput.x > 0f)
        {
            FlipPlayer();
        }
        else if (isFacingRight && movementInput.x < 0f)
        {
            FlipPlayer();
        }

        if (movementInput == Vector2.zero)
        {
            movementThisFrame *= DeccelFactor;
        }

        playerRigidbody.velocity = movementThisFrame + new Vector2(0, playerRigidbody.velocity.y);
    }

    void Jump()
    {
        //playerRigidbody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpHeight);
    }

    public void UpdateMovementData(Vector2 newMovementDirection)
    {
        movementInput = newMovementDirection;
    }

    public void Dash()
    {
        float dashDistanceThisFrame = DashDistance;

        if (!isFacingRight)
        {
            dashDistanceThisFrame = -dashDistanceThisFrame;
        }

        //playerRigidbody.AddForce(Vector2.right * dashDistanceThisFrame);
        playerRigidbody.velocity = new Vector2(dashDistanceThisFrame, playerRigidbody.velocity.y);
        canDash = false;
    }

    public void UpdateJump()
    {
        if (canJump)
        {
            Debug.Log("Jump");
            Jump();
            canJump = false;
            isGrounded = false;
        } else
        {
            if (canDoubleJump)
            {
                Debug.Log("Double jump");
                Jump();
                canDoubleJump = false;
                isGrounded = false;
            }
            
        }
    }

    public void UpdateDash()
    {
        
        Dash();

        if (!dashTimerOn)
        {
            dashTimerOn = true;
            StartCoroutine(DashCoroutine());
        }
    }

    void FlipPlayer()
    {
        // Flip the player
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            canDoubleJump = true;
            isGrounded = true;
        }
    }

    //void UpdateGrouding()
    //{
    //    if (playerCollider.IsTouchingLayers(groundMask))
    //    {
    //        isGrounded = true;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //    }
    //}

    void UpdateGravity()
    {
        if (playerRigidbody.velocity.y <= 0f)
        {
            //Debug.Log("Gravity scale set");
            playerRigidbody.gravityScale = FallingGravityScale;
        } else
        {
        //    playerRigidbody.gravityScale = 1;

        }

        if (isGrounded)
        {
       //     playerRigidbody.gravityScale = 1;
        }
    }

    IEnumerator DashCoroutine()
    {
        Debug.Log("Dash coroutine started");

        yield return new WaitForSeconds(DashTimer);
        canDash = true;
        dashTimerOn = false;
        playerController.SwitchState(InariState.Neutral);
        
    }
}

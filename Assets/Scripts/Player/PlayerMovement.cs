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


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        groundMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //UpdateGrouding();
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

        playerRigidbody.velocity = movementThisFrame;
    }

    void Jump()
    {
        playerRigidbody.AddForce(Vector2.up * JumpHeight);
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

        playerRigidbody.AddForce(Vector2.right * dashDistanceThisFrame);
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
            else
            {
                if (canWallJump)
                {
                    Jump();
                    canWallJump = false;
                }
            }
        }
    }

    public void UpdateDash()
    {
        if (canDash)
        {
            Dash();

            if (!dashTimerOn)
            {
                dashTimerOn = true;
                StartCoroutine(DashCoroutine());
            }
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

    }

    IEnumerator DashCoroutine()
    {
        Debug.Log("Dash coroutine started");

        yield return new WaitForSeconds(DashTimer);
        canDash = true;
        dashTimerOn = false;
    }
}

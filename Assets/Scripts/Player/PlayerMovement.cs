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

    private bool canMove;
    private bool canJump;
    private bool isFacingRight;
    private Vector2 movementInput;

    Rigidbody2D playerRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        canMove = true;
        canJump = true;
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        MovePlayer();
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

    public void UpdateJump(float isJumping)
    {
        Debug.Log("Jump method");
        bool isJumpingThisFrame = isJumping > 0;

        if (canJump && isJumpingThisFrame)
        {
            Jump();
            canJump = false;
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
        }
    }
}

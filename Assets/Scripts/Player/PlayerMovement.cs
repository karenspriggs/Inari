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

    private bool canMove;
    private Vector2 movementInput;

    Rigidbody2D playerRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        canMove = true;
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

        Vector2 movementThisFrame = new Vector2(movementInput.x, movementInput.y) * MovementSpeed * Time.deltaTime;

        if (movementInput == Vector2.zero)
        {
            movementThisFrame *= DeccelFactor;
        }

        playerRigidbody.velocity = movementThisFrame;
    }

    public void UpdateMovementData(Vector2 newMovementDirection)
    {
        movementInput = newMovementDirection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorBasicAttackBool;

    public float velocityToStopMovingAnim;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorBasicAttackBool = Animator.StringToHash("IsBasicAttacking");
    }

    public void UpdateMoveAnimation(Vector2 moveInput)
    {
        bool isMoving = true;

        if ((moveInput.x < velocityToStopMovingAnim && moveInput.x >= -velocityToStopMovingAnim) && (moveInput.y < velocityToStopMovingAnim && moveInput.y >= -velocityToStopMovingAnim))
        {
            isMoving = false;
        }

        animator.SetBool(animatorMoveBool, isMoving);
    }

    public void UpdateJumpAnimation(float isJumping)
    {

    }

    public void UpdateBaseAttackAnimation(float isAttacking)
    {
        bool isAttackingThisFrame = true;

        if (isAttacking == 0f) isAttackingThisFrame = false;

        animator.SetBool(animatorBasicAttackBool, isAttackingThisFrame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

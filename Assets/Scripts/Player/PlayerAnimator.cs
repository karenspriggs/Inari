using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorBasicAttackBool;
    private int animatorJumpTrigger;
    private int animatorDashTrigger;

    public float velocityToStopMovingAnim;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorBasicAttackBool = Animator.StringToHash("IsBasicAttacking");
        animatorJumpTrigger = Animator.StringToHash("IsJumping");
        animatorDashTrigger = Animator.StringToHash("IsDashing");
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

    public void UpdateJumpAnimation()
    {
        Debug.Log("Jump animation");
        animator.SetTrigger(animatorJumpTrigger);
    }

    public void UpdateDashAnimation()
    {
        Debug.Log("Dash animation");
        animator.SetTrigger(animatorDashTrigger);
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

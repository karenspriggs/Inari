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
        animator = GetComponentInChildren<Animator>();
        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorBasicAttackBool = Animator.StringToHash("IsBasicAttacking");
        animatorJumpTrigger = Animator.StringToHash("IsJumping");
        animatorDashTrigger = Animator.StringToHash("IsDashing");
    }

    public void UpdateMoveAnimation(float moveInput)
    {
        bool isMoving = true;

        if ((moveInput < velocityToStopMovingAnim && moveInput >= -velocityToStopMovingAnim))
        {
            isMoving = false;
        }

        animator.SetBool(animatorMoveBool, isMoving);
    }

    public void UpdateJumpAnimation()
    {
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

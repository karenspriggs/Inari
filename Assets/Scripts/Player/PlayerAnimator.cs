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
        //animatorBasicAttackBool = Animator.StringToHash("IsBasicAttacking");
        //animatorJumpTrigger = Animator.StringToHash("IsJumping");
        //animatorDashTrigger = Animator.StringToHash("IsDashing");
    }

    public void AnimationUpdateMoveBool(float moveInput)
    {
        bool isMoving = true;

        if ((moveInput < velocityToStopMovingAnim && moveInput >= -velocityToStopMovingAnim))
        {
            isMoving = false;
        }

        animator.SetBool(animatorMoveBool, isMoving);
    }
    /*
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
    */

    public void SwitchState(InariState newState)
    {
        switch (newState)
        {
            case InariState.Neutral:
                StartAnimation("PlayerDollIdle");
                break;
            case InariState.DashStartup:
                StartAnimation("PlayerDollDash");
                break;
            case InariState.Dashing:
                break;
            case InariState.Jumping:
            case InariState.DoubleJumping:
                StartAnimation("PlayerDollJump");
                break;
            case InariState.Air:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// starts the animation
    /// </summary>
    /// <param name="animationStateName"></param>
    public void StartAnimation(string animationStateName)
    {
        StartAnimation(animationStateName, 0.0f);
    }

    /// <summary>
    /// Starts the animation at a given starting point of the animation.
    /// </summary>
    /// <param name="animationStateName"></param>
    /// <param name="normalizedTime"> Starts the animation part way through. 0.0 is begining, 1.0 is end, 0.5 is 50% through etc.</param>
    public void StartAnimation(string animationStateName, float normalizedTime)
    {
        animator.Play(animationStateName, -1, normalizedTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

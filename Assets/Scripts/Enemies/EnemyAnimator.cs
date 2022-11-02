using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemySound enemySound;
    float velocityToStopMovingAnim;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorChaseBool;
    private int animatorWanderBool;
    private int animatorAttackTrigger;
    private int animatorHitTrigger;
    private int animatorDeathTrigger;

    public string AnimatorIdleStateName;
    public string AnimatorIdleAltStateName;
    public string AnimatorWalkStateName;
    public string AnimatorChaseStateName;
    public string AnimatorHitStateName;
    public string AnimatorAttackStateName;
    public string AnimatorStunStateName;
    public string AnimatorDeathStateName;

    public int StunTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemySound = GetComponent<EnemySound>();

        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorWanderBool = Animator.StringToHash("IsWandering");
        animatorChaseBool = Animator.StringToHash("IsChasing");
        animatorHitTrigger = Animator.StringToHash("IsHit");
        animatorDeathTrigger = Animator.StringToHash("IsDead");
        animatorAttackTrigger = Animator.StringToHash("IsAttacking");
    }

    public void StartHitAnimation()
    {
        Debug.Log("Hit animation");
        animator.SetTrigger(animatorHitTrigger);
        enemySound.PlaySound(enemySound.HitSound);
    }
    public void StartDeathAnimation()
    {
        animator.SetTrigger(animatorDeathTrigger);
        enemySound.PlaySound(enemySound.DeathSound);
    }

    public void UpdateMoveAnimation(bool isMoving)
    {
        animator.SetBool(animatorMoveBool, isMoving);
    }

    public void StartAttackAnimation()
    {
        animator.SetTrigger("IsAttacking");
        enemySound.PlaySound(enemySound.AttackSound);
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

    public void StartAnimation(string animationStateName)
    {
        StartAnimation(animationStateName, 0.0f);
    }

    public void StartAnimation(string animationStateName, float normalizedTime)
    {
        animator.Play(animationStateName, -1, normalizedTime);
    }

    public bool CheckIfAnimationEnded()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }

    void DetermineIdleAnimation()
    {
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            StartAnimation(AnimatorIdleStateName);
        } else
        {
            StartAnimation(AnimatorIdleAltStateName);
        }
    }

    public void SwitchState(EnemyState newState)
    {
        switch (newState)
        {
            case (EnemyState.Idle):
                DetermineIdleAnimation();
                break;
            case (EnemyState.Wander):
                StartAnimation(AnimatorWalkStateName);
                break;
            case (EnemyState.Chase):
                StartAnimation(AnimatorChaseStateName);
                break;
            case (EnemyState.Attack):
                StartAnimation(AnimatorAttackStateName);
                break;
            case (EnemyState.Hit):
                StartAnimation(AnimatorHitStateName);
                break;
            case (EnemyState.Stun):
                StartAnimation(AnimatorStunStateName, StunTime);
                break;
            case (EnemyState.Death):
                StartAnimation(AnimatorDeathStateName);
                break;
        }
    }
}

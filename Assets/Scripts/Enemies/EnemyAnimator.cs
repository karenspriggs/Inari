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

    public bool CheckIfAnimationEnded()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }

    public void SwitchState(EnemyState newState)
    {
        switch (newState)
        {
            case (EnemyState.Idle):
                animator.SetBool(animatorChaseBool, false);
                animator.SetBool(animatorWanderBool, false);
                break;
            case (EnemyState.Wander):
                animator.SetBool(animatorWanderBool, true);
                animator.SetBool(animatorChaseBool, false);
                break;
            case (EnemyState.Chase):
                animator.SetBool(animatorChaseBool, true);
                animator.SetBool(animatorWanderBool, false);
                break;
            case (EnemyState.Attack):
                animator.SetTrigger("IsAttacking");
                enemySound.PlaySound(enemySound.AttackSound);
                break;
            case (EnemyState.Hit):
                animator.SetTrigger(animatorHitTrigger);
                enemySound.PlaySound(enemySound.HitSound);
                break;
            case (EnemyState.Death):
                animator.SetTrigger(animatorDeathTrigger);
                enemySound.PlaySound(enemySound.DeathSound);
                break;
        }
    }
}

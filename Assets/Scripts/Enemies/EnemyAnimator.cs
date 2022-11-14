using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemySound enemySound;
    float velocityToStopMovingAnim;

    public string AnimatorIdleStateName;
    public string AnimatorIdleAltStateName;
    public string AnimatorWalkStateName;
    public string AnimatorChaseStartupStateName;
    public string AnimatorChaseStateName;
    public string AnimatorHitStateName;
    public string AnimatorAttackStateName;
    public string AnimatorStunStateName;
    public string AnimatorLaunchDeathStateName;
    public string AnimatorDeathStateName;

    public int StunTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemySound = GetComponent<EnemySound>();
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
            case (EnemyState.ChaseStartup):
                StartAnimation(AnimatorChaseStartupStateName);
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
            case (EnemyState.DeadLaunch):
                StartAnimation(AnimatorLaunchDeathStateName);
                break;
            case (EnemyState.Death):
                StartAnimation(AnimatorDeathStateName);
                break;
        }
    }
}

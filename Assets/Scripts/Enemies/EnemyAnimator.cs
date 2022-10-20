using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemySound enemySound;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorAttackTrigger;
    private int animatorHitTrigger;
    private int animatorDeathTrigger;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemySound = GetComponent<EnemySound>();

        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorHitTrigger = Animator.StringToHash("IsHit");
        animatorDeathTrigger = Animator.StringToHash("IsDead");
        animatorAttackTrigger = Animator.StringToHash("IsAttacking");
    }

    public void UpdateMoveAnimation(bool isMoving)
    {
        animator.SetBool(animatorMoveBool, isMoving);
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

    public void StartAttackAnimation()
    {
        animator.SetTrigger("IsAttacking");
        enemySound.PlaySound(enemySound.AttackSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

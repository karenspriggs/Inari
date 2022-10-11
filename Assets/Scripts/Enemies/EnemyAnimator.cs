using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorAttackBool;
    private int animatorHitTrigger;
    private int animatorDeathTrigger;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorHitTrigger = Animator.StringToHash("IsHit");
        animatorDeathTrigger = Animator.StringToHash("IsDead");
    }

    public void UpdateMoveAnimation(bool isMoving)
    {
        animator.SetBool(animatorMoveBool, isMoving);
    }

    public void StartHitAnimation()
    {
        animator.SetTrigger(animatorHitTrigger);
    }

    public void StartDeathAnimation()
    {
        animator.SetTrigger(animatorDeathTrigger);
    }

    public void StartAttackAnimation()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

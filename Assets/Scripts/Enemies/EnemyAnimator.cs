using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("Animator Values")]
    private int animatorMoveSpeed;
    private int animatorAttackBool;
    private int animatorHitTrigger;
    private int animatorDeathTrigger;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        animatorMoveSpeed = Animator.StringToHash("MoveSpeed");
        animatorHitTrigger = Animator.StringToHash("IsHit");
        animatorDeathTrigger = Animator.StringToHash("IsDead");
    }

    public void UpdateMoveAnimation(float _moveSpeed)
    {
        animator.SetFloat(animatorMoveSpeed, _moveSpeed);
    }

    public void StartHitAnimation()
    {
        animator.SetTrigger(animatorHitTrigger);
    }

    public void StartDeathAnimation()
    {
        animator.SetTrigger(animatorDeathTrigger);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

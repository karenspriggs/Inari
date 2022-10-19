using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static Action PlayerDeathAnimationFinished;
    public static Action PlayerAttackActiveStarted;
    public static Action PlayerAttackRecoveryStarted;

    public void CallDeathEvent()
    {
        //Debug.Log("Death event called");
        PlayerDeathAnimationFinished?.Invoke();
    }

    public void CallAttackActiveEvent()
    {
        PlayerAttackActiveStarted?.Invoke();
    }

    public void CallAttackRecoveryEvent()
    {
        PlayerAttackRecoveryStarted?.Invoke();
    }
}

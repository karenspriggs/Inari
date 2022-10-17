using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDeathAnimationEvents : MonoBehaviour
{
    public static Action PlayerDeathAnimationFinished;

    public void CallDeathEvent()
    {
        Debug.Log("Death event called");
        PlayerDeathAnimationFinished?.Invoke();
    }
}

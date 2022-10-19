using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationVelocityEventArgs : EventArgs
{
    public Vector2 velocity;

    public AnimationVelocityEventArgs(Vector2 _velocity)
    {
        velocity = _velocity;
    }
}

public class PlayerAnimationEvents : MonoBehaviour
{
    public static Action PlayerDeathAnimationFinished;
    public static Action PlayerAttackActiveStarted;
    public static Action PlayerAttackRecoveryStarted;
    public static event EventHandler<AnimationVelocityEventArgs> PlayerAnimationVelocityImpulse;

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

    public void CauseVelocityImpulseX(float xVelocity)
    {
        CauseVelocityImpulse(xVelocity, 0);
    }
    public void CauseVelocityImpulseY(float yVelocity)
    {
        CauseVelocityImpulse(0, yVelocity);
    }

    public void CauseVelocityImpulse(float xVelocity, float yVelocity)
    {
        Vector2 v = new Vector2(xVelocity, yVelocity);
        AnimationVelocityEventArgs e = new AnimationVelocityEventArgs(v);

        PlayerAnimationVelocityImpulse?.Invoke(this, e);
    }
}

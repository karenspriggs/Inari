using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : SoundPlayer
{
    public enum AttackSounds
    {
        Basic,
        Heavy
    }

    public AudioClip AttackSound;
    public AudioClip HeavyAttackSound;
    public AudioClip JumpSound;
    public AudioClip LandingSound;
    public AudioClip DashSound;
    public AudioClip HitSound;
    public AudioClip DeathSound;

    public void PlayAttackSound(AttackSounds attack)
    {
        switch (attack)
        {
            case AttackSounds.Basic:
                PlaySound(AttackSound);
                break;
            case AttackSounds.Heavy:
                PlaySound(HeavyAttackSound);
                break;
        }
    }
}

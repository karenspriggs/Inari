using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemyShootEvent : MonoBehaviour
{
    public static Action ShootProjectileInitiated;

    public void ShootProjectileActive()
    {
        ShootProjectileInitiated?.Invoke();
    }
}

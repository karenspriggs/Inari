using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public void EnemyDeathEvent()
    {
        Destroy(transform.parent.gameObject);
    }
}

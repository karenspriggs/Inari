using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EnemyDeath : MonoBehaviour
{
    public void EnemyDeathEvent()
    {
        Destroy(transform.parent.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EnemyDeath : MonoBehaviour
{
    public KillCount count;

    private void Start()
    {
        count = FindObjectOfType<KillCount>();
    }
    public void EnemyDeathEvent()
    {
        Destroy(transform.parent.gameObject);
        count.AddKills();
        //This was supposed to just add to the counter once the ememy died
    }
}

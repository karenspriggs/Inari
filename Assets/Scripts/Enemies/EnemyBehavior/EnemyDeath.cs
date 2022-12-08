using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    //public KillCount count;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeathEvent()
    {
        Destroy(transform.parent.gameObject);
        //count.AddKills();

    }
}

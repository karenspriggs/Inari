using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    //KillCount killCounter;


    void Start()
    {
        //killCounter = GameObject.Find("kill count").GetComponent <KillCount>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeathEvent()
    {
        Destroy(transform.parent.gameObject);

       // killCounter.AddKills();

    }
}

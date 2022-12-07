using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    KillCount killCountscript;


    void Start()
    {
        killCountscript=GameObject.Find("KCO").GetComponent<KillCount>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeathEvent()
    {
        Destroy(transform.parent.gameObject);
        killCountscript.AddKills();
    }
}

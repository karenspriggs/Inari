using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContactDamage : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool damagePlayer;
    private PlayerData playerdata;
    private EnemyData enemydata;
    

    private void Start()
    {
        enemydata = this.GetComponent<EnemyData>();
        playerdata = player.GetComponent<PlayerData>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"dealt {damage} damage to {collision.gameObject.name}");
        if (collision.gameObject.name == player.name && damagePlayer)
        { DamagePlayer(); }
        else
        {
          DamageEnemy();
        }
        Destroy(gameObject);
    }  
    
    private void DamagePlayer()
    {
        playerdata.TakeDamage(damage);
    }

    private void DamageEnemy()
    {
        enemydata.TakeDamage(damage);
    }

}

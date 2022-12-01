using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnEffect : MonoBehaviour
{
    public GameObject enemyToSpawn;
    ParticleSystem spawnParticles;

    // Start is called before the first frame update
    void Start()
    {
        spawnParticles = GetComponent<ParticleSystem>();
        enemyToSpawn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayEffects()
    {
        spawnParticles.Play();
    }

    public void TurnOnEnemy()
    {
        enemyToSpawn.gameObject.SetActive(true);
        PlayEffects(); 
    }
}

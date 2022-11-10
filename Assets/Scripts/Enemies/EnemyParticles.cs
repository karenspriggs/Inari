using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticles : MonoBehaviour
{
    public GameObject hitParticles;
    public ParticleSystem hitParts;
    public TimeManager timeManager;

    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    public void PlayHitParticles()
    {
        hitParts.Play();
        timeManager.DoHitStop();
    }
}

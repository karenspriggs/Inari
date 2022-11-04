using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticles : MonoBehaviour
{
    public GameObject hitParticles;
    public ParticleSystem hitParts;

    public void PlayHitParticles()
    {
        hitParts.Play();
    }
}

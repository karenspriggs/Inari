using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public ParticleSystem dust;

    public void CreateDust()
    {
        dust.Play();
    }
}

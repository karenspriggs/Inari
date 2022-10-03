using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardFade : MonoBehaviour
{
    [SerializeField] private float delay;
    void Start()
    {
        Destroy(this.gameObject, delay);
    }
}

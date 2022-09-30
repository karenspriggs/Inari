using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardWarning : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private GameObject hazard;

    /*
    void Start()
    {
        Invoke("CreateHazard", delay);
    } */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //this bit is just to make testing easier
        Invoke("CreateHazard", delay);
    }

    void CreateHazard()
    {
        Instantiate(hazard);
    }
}

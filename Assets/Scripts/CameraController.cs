using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float smoothSpeed = 0.125f;
    public Vector3 offest;

    public GameObject wall;
    public Transform place;

    private void Start()
    {
        transform.LookAt(player);
    }

    void FixedUpdate()
    {
        //moving camera with player.
        Vector3 cameraPosition = player.position + offest;

        Vector3 smoothPos = Vector3.Lerp(transform.position, cameraPosition, smoothSpeed);
        transform.position = smoothPos;

        

        //goes back to following player once enimes all die.
    
    }
        //stops at blocker
    public void OnTriggerEnter(Collider other)
    {
        
            smoothSpeed = 0;
            transform.LookAt(place);
            
            
    }
}

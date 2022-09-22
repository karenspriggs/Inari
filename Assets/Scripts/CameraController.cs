using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offest;

    void FixedUpdate()
    {
        //moving camera with player.
        Vector3 cameraPosition = target.position + offest;
        Vector3 smoothPos = Vector3.Lerp(transform.position, cameraPosition, smoothSpeed);

        transform.position = smoothPos;
        transform.LookAt(target);
        //stops at blocker
        //goes back to following player once enimes all die.
    
    }
}

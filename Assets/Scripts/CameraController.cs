using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] playerTransform;

    private void Start()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerTransform = new Transform[allPlayers.Length];
        for (int i = 0; i < allPlayers.Length; i++) 
        { 
            playerTransform[i] = allPlayers[i].transform;
        }
    }

    public float yOffset = 2.0f;
    public float minDistance = 7.5f;

    private float xMin,xMax, yMin, yMax;

    void LateUpdate()
    {
        if(playerTransform.Length == 0)
        {
            
            return;
        }
        xMin = xMax = playerTransform[0].position.x;
        yMin = yMax = playerTransform[0].position.y;
        for (int i = 1; i < playerTransform.Length; i++)
        {
            if (playerTransform[i].position.x < xMin) 
                xMin = playerTransform[i].position.x;

            if (playerTransform[i].position.x > xMax)
                xMax = playerTransform[i].position.x;

            if (playerTransform[i].position.x < yMin)
                yMin = playerTransform[i].position.y;

            if (playerTransform[i].position.x > yMax)
                yMax = playerTransform[i].position.y;

        }
        float xMiddle = (xMax + xMin) / 2;
        float yMiddle = (yMax + yMin) / 2;
        float distance = xMax - xMin;
        if (distance< minDistance)
            distance = minDistance;
    }
       
    public void OnTriggerEnter(Collider other)
    {
            
            
    }
}

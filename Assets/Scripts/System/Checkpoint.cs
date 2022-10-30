using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID = 0;
    public bool hasActivated = false;
    private Light2D checkpointLight;

    private BoxCollider2D checkpointCollider;

    private void Start()
    {
        checkpointLight = GetComponent<Light2D>();
        checkpointLight.enabled = false;
        checkpointCollider = GetComponent<BoxCollider2D>();
    }

    void EnableCheckpoint()
    {
        checkpointLight.enabled = true;
        hasActivated = true;
        checkpointCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated && collision.CompareTag("Player"))
        {
            EnableCheckpoint();
            collision.GetComponent<PlayerData>().currentCheckpointID = this.checkpointID;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID = 0;
    public bool hasActivated = false;
    public bool shouldLightUp = true;

    private Light2D checkpointLight;

    private BoxCollider2D checkpointCollider;

    private void Start()
    {
        checkpointLight = GetComponentInChildren<Light2D>();
        checkpointLight.enabled = false;
        checkpointCollider = GetComponent<BoxCollider2D>();

        if (PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID > this.checkpointID)
        {
            DisableCheckpoint();
        }
    }

    void UseCheckpoint()
    {
        DisableCheckpoint();
        PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID = this.checkpointID;
        PlayerSaveSystem.SaveGame();
    }

    public void DisableCheckpoint()
    {
        if (shouldLightUp)
        {
            checkpointLight.enabled = true;
        }
        hasActivated = true;
        checkpointCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated && collision.CompareTag("Player"))
        {
            UseCheckpoint();
            collision.GetComponent<PlayerData>().currentCheckpointID = this.checkpointID;
        }
    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID = 0;
    public float lanternIntensityStepTime;
    public int lanternIntensity = 18;
    public bool hasActivated = false;
    public bool shouldLightUp = true;

    private Light2D checkpointLight;
    private LightAnimator lanternAnimator;

    private BoxCollider2D checkpointCollider;

    public static Action PlayerPassedCheckpoint;

    private void Start()
    {
        checkpointLight = GetComponentInChildren<Light2D>();
        checkpointLight.enabled = false;
        checkpointCollider = GetComponent<BoxCollider2D>();
        lanternAnimator = GetComponent<LightAnimator>();

        if (PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID >= this.checkpointID)
        {
            DisableCheckpoint();
        }
    }

    void UseCheckpoint()
    {
        DisableCheckpoint();
        PlayerPassedCheckpoint?.Invoke();
        PlayerSaveSystem.SessionSaveData.playerStats.LatestCheckpointID = this.checkpointID;
        PlayerSaveSystem.SaveGame();
    }

    public void DisableCheckpoint()
    {
        if (shouldLightUp)
        {
            checkpointLight.enabled = true;
            lanternAnimator.StartFadeAnimation();
        }
        else
        {
            if (checkpointID != 0)
            {
                lanternAnimator.StartLightOnAnimation();
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> CheckpointsInLevel;

    public static Action PlayerRespawnedAtCheckpoint;

    public Vector3 ReturnCheckpointTransform(int checkpointID)
    {
        foreach (Checkpoint checkpoint in CheckpointsInLevel)
        {
            if (checkpoint.checkpointID == checkpointID)
            {
                if (checkpointID != 0)
                {
                    PlayerRespawnedAtCheckpoint?.Invoke();
                }

                return checkpoint.gameObject.transform.position;
            }
        }

        return CheckpointsInLevel[0].gameObject.transform.position;
    }
}

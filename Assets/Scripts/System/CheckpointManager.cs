using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> CheckpointsInLevel;

    public Vector3 ReturnCheckpointTransform(int checkpointID)
    {
        foreach (Checkpoint checkpoint in CheckpointsInLevel)
        {
            if (checkpoint.checkpointID == checkpointID)
            {
                return checkpoint.gameObject.transform.position;
            }
        }

        return CheckpointsInLevel[0].gameObject.transform.position;
    }
}

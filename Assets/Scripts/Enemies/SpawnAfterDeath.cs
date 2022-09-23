using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObject;

    void OnDestroy()
    {
        Instantiate(spawnObject);
        Debug.Log($"spawned {spawnObject.name}");
    }

}

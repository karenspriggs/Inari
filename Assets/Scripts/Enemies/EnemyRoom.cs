using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    public GameObject[] ActivationObjects;
    public BoxCollider2D[] WallColliders;
    public EnemyRoomWave[] enemyWaves;

    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    
    public bool isActivated;
    public int currentWaveIndex = 0;
    public int waveCount;

    private void Start()
    {
        isActivated = false;

        foreach (GameObject obj in ActivationObjects)
        {
            obj.SetActive(false);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        waveCount = enemyWaves.Length;

        audioSource = GetComponent<AudioSource>();
    }

    void ActivateRoom()
    {
        foreach(GameObject obj in ActivationObjects)
        {
            obj.SetActive(true);
        }

        StartNextWave();

        spriteRenderer.enabled = true;

        audioSource.Play();
    }

    void CheckIfShouldDeactivate()
    {
        if (waveCount == 0)
        {
            DeactivateRoom();
        } else
        {
            currentWaveIndex++;
            StartNextWave();
        }
    }

    void DeactivateRoom()
    {
        foreach (GameObject obj in ActivationObjects)
        {
            obj.SetActive(false);
        }

        spriteRenderer.enabled = false;
    }

    public void ClearWave()
    {
        waveCount--;
        CheckIfShouldDeactivate();
    }

    void StartNextWave()
    {
        enemyWaves[currentWaveIndex].StartWave();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            ActivateRoom();
        }
    }
}

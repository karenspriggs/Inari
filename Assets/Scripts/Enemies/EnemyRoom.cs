using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    public GameObject[] ActivationObjects;
    public List<EnemySpawnEffect> Enemies;
    public BoxCollider2D[] WallColliders;

    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    

    public bool isActivated;
    public int enemyCount;

    private void OnEnable()
    {
        EnemyData.EnemyKilled += DecreaseEnemyCount;
    }

    private void OnDisable()
    {
        EnemyData.EnemyKilled -= DecreaseEnemyCount;
    }

    private void Start()
    {
        isActivated = false;

        foreach (GameObject obj in ActivationObjects)
        {
            obj.SetActive(false);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        audioSource = GetComponent<AudioSource>();

        foreach(EnemySpawnEffect spawner in GetComponentsInChildren<EnemySpawnEffect>())
        {
            Enemies.Add(spawner);
        }
    }

    void ActivateRoom()
    {
        foreach(GameObject obj in ActivationObjects)
        {
            obj.SetActive(true);
        }

        foreach(EnemySpawnEffect enemy in Enemies)
        {
            enemy.TurnOnEffect();
        }

        spriteRenderer.enabled = true;

        audioSource.Play();
    }

    void DecreaseEnemyCount() 
    {
        if (isActivated)
        {
            enemyCount--;
            CheckIfShouldDeactivate();
        }
    }

    void CheckIfShouldDeactivate()
    {
        if (enemyCount == 0)
        {
            DeactivateRoom();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            ActivateRoom();
        }
    }
}

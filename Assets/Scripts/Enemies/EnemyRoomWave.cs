using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomWave : MonoBehaviour
{
    [SerializeField]
    EnemyRoom enemyRoom;

    public List<EnemySpawnEffect> Enemies;
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

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;

        foreach (EnemySpawnEffect spawner in GetComponentsInChildren<EnemySpawnEffect>())
        {
            Enemies.Add(spawner);
        }

        enemyCount = Enemies.Count;
    }
    
    public void StartWave()
    {
        foreach (EnemySpawnEffect enemy in Enemies)
        {
            enemy.TurnOnEffect();
        }

        isActivated = true;
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
            EndWave();
        }
    }

    void EndWave()
    {
        enemyRoom.ClearWave();
        Debug.Log("Player cleared the wave");
        isActivated = false;
    }
}

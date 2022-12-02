using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnEffect : MonoBehaviour
{
    public GameObject enemyToSpawn;
    int animatorPoofTrigger;
    Animator effectAnimator;

    // Start is called before the first frame update
    void Start()
    {
        effectAnimator = GetComponent<Animator>();
        animatorPoofTrigger = Animator.StringToHash("enemyPoofOn");
        enemyToSpawn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnEffect()
    {
        effectAnimator.SetTrigger(animatorPoofTrigger);
    }

    public void TurnOnEnemy()
    {
        enemyToSpawn.gameObject.SetActive(true);
    }
}

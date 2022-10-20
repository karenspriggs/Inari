using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFood : MonoBehaviour
{
    public FoodSound sounds;

    private void Start()
    {
        sounds.audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sounds.PlaySound(sounds.pickedUp);
            TurnOffFood();
        }
    }
    
    void TurnOffFood()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}

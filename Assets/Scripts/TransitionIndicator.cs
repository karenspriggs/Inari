using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionIndicator : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            sprite.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            sprite.enabled = false;
        }
    }
}

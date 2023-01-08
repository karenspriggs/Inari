using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectible : MonoBehaviour
{
    public int collectibleIndex;

    bool hasBeenCollected = false;
    SpriteRenderer spriteRenderer;
    BoxCollider2D triggerCollider;

    public static Action<int> CollectibleObtained;

    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        CollectibleObtained?.Invoke(collectibleIndex);
        spriteRenderer.enabled = false;
        triggerCollider.enabled = false;
        hasBeenCollected = true;
    }
}

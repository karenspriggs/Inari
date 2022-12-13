using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public Sprite brokenSprite;

    bool isBroken = false;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox") || collision.gameObject.CompareTag("HeavyHitbox") && !isBroken)
        {
            Physics2D.IgnoreCollision(boxCollider2D, collision.GetComponentInParent<CapsuleCollider2D>());
            isBroken = true;
            spriteRenderer.sprite = brokenSprite;
        }
    }
}

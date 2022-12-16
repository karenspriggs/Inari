using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : SoundPlayer
{
    public Sprite brokenSprite;
    public AudioClip breakSound;
    public ParticleSystem breakParticles;

    bool isBroken = false;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;
    Rigidbody2D rigidbody2D;


    // Start is called before the first frame update
    new void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox") || collision.gameObject.CompareTag("HeavyHitbox") && !isBroken)
        {
            Physics2D.IgnoreCollision(capsuleCollider2D, collision.GetComponentInParent<CapsuleCollider2D>());
            isBroken = true;
            spriteRenderer.sprite = brokenSprite;

            PlaySound(breakSound);
            breakParticles.Play();
        }
    }
}

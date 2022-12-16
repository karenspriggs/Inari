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

    public GameObject hiddenObject;

    // Start is called before the first frame update
    new void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitbox") || collision.gameObject.CompareTag("HeavyHitbox") || collision.gameObject.CompareTag("LaunchHitbox"))
        {
            if (!isBroken)
            {
                //first time break
                Physics2D.IgnoreCollision(capsuleCollider2D, collision.GetComponentInParent<CapsuleCollider2D>());
                isBroken = true;
                spriteRenderer.sprite = brokenSprite;

                // TODO: put this on a better layer probably. not just the default one?
                this.gameObject.layer = 0; // stop being ground. 0 is default layer. this stops inari from trying to land on it
                

                //reveal hidden object if there is one
                if (hiddenObject != null)
                {
                    hiddenObject.SetActive(true);
                }
            }

            //any time hit, play the sound and particles
            PlaySound(breakSound);
            breakParticles.Play();
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public FoodSound sounds;
    public Item Item;
    // Start is called before the first frame update
    private void Start()
    {
        sounds.audioSource = GetComponent<AudioSource>();
    }

    void PickUp()
    {
        PlayerInventory.Instance.Add(Item);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sounds.PlaySound(sounds.pickedUp);
            PickUp();
        }
    }
}

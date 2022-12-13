using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        var quickSlot = GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>();
        PlayerInventory.Instance.Add(Item);
        if (quickSlot.itemcontroller.item == null)
        {
            quickSlot.SetQuickSlot(Item);
        }
            else if (quickSlot.itemcontroller.item == Item)
        {
            quickSlot.itemcontroller.ItemRefresh();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [System.NonSerialized]
    public Item item;
    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerData>().HealHealth(item.health);
        GameObject.Find("InventoryManager").GetComponent<PlayerInventory>().Remove(item);
        GameObject.Find("InventoryManager").GetComponent<PlayerInventory>().ListItems();
    }
}

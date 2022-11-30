using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [System.NonSerialized]
    private Item item;
    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        
        GameObject.FindWithTag("Player").GetComponent<PlayerData>().HealHealth(item.health);
        GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Remove(item);
        GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().ListItems();
        var itemCount = transform.Find("ItemCount").GetComponent<TMPro.TextMeshProUGUI>();
        itemCount.text = GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Items.Count(i => i == item).ToString();

    }
    
    public void SetQuickSlot()
    {
        GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>().SetQuickSlot(item);
    }


}

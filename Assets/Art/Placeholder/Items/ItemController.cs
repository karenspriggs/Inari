using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [System.NonSerialized]
    private Item item;
    public Image icon;
    public TextMeshProUGUI count;
    public TextMeshProUGUI itemname;
    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerData>().HealHealth(item.health);
        GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Remove(item);
        GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().ListItems();
        ItemRefresh();
    }
    
    public void ItemRefresh()
    {
        count.text = GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Items.Count(i => i == item).ToString();
        itemname.text = item.itemName;
        icon.sprite = item.icon;
    }

    public void SetQuickSlot()
    {
        GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>().SetQuickSlot(item);
    }


}

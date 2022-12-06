using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [System.NonSerialized]
    public Item item;
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
        int count = GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Items.Count(i => i == item);
        if (count <= 0)
        {
            GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>().itemcontroller.ClearItem();
        }
        else
        {
            ItemRefresh();
        }
    }
    
    public void ItemRefresh()
    {
        count.text = GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Items.Count(i => i == item).ToString();
        itemname.text = item.itemName;
        icon.sprite = item.icon;
    }

    public void ClearItem()
    {
        icon.sprite = null;
        itemname.text = "";
        count.text = "";
    }


    public void SetQuickSlot()
    {
        GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>().SetQuickSlot(item);
    }


}

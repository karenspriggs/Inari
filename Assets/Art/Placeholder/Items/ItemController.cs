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
    public Sprite quickslot;
    public TextMeshProUGUI count;
    public TextMeshProUGUI itemname;
    public TextMeshProUGUI value;
    public void AddItem(Item newItem)
    {
        item = newItem;
    }
    public bool CanUseItem()
    {
        return !(item == null);
    }
    public void UseItem()
    {
        if (item == null) return;
        GameObject.FindWithTag("Player").GetComponent<PlayerData>().HealHealth(item.health);
        GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Remove(item);
        GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().ListItems();
        int _count = GameObject.FindWithTag("Inventory").GetComponent<PlayerInventory>().Items.Count(i => i == item);
        if (_count <= 0)
        {
            ClearItem();
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
        value.text = "Heal Value: " + item.health.ToString();
    }

    public void ClearItem()
    {
        icon.sprite = quickslot;
        itemname.text = "";
        count.text = "";
        item = null;
    }


    public void SetQuickSlot()
    {
        GameObject.FindWithTag("QuickSlot").GetComponent<QuickSlot>().SetQuickSlot(item);
    }


}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public List<Item> Items = new List<Item>();
    // Start is called before the first frame update

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ListItems()
    {
        
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items.Distinct())
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            obj.GetComponent<ItemController>().item = item;

            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemImage").GetComponent<Image>();
            var itemCount = obj.transform.Find("ItemCount").GetComponent<TMPro.TextMeshProUGUI>();
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemCount.text = Items.Count(i => i == item).ToString();
        }
    }
}

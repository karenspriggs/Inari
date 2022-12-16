using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public ItemController itemcontroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetQuickSlot(Item i)
    {
        itemcontroller.AddItem(i);
        itemcontroller.ItemRefresh();
    }

    public void UseItem()
    {
        itemcontroller.UseItem();
    }
    public bool CanUseItem()
    {
        return itemcontroller.CanUseItem();
    }
}

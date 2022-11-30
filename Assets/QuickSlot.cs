using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public Image Icon;
    public ItemController itemcontroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetQuickSlot(Item i)
    {
        itemcontroller.AddItem(i);
        Icon.sprite = i.icon;
    }

    public void UseItem()
    {
        itemcontroller.UseItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

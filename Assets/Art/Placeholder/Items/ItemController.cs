using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
    }
}

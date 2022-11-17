using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public int health;
    //public override bool Equals(object other)
    //{
    //    var item = other as Item;
    //    return this.id.Equals(item.id);
    //}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData item;
    public int stack;

    public InventoryItem(ItemData _item)
    {
        this.item = _item;
        AddStack();
    }

    public void AddStack() => this.stack++;
    public void RemoveStack() => this.stack--;
}

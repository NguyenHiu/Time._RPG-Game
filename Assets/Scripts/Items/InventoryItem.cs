using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stack;

    public InventoryItem(ItemData _item)
    {
        this.data = _item;
        AddStack();
    }

    public void AddStack() => this.stack++;
    public void RemoveStack(int amount = 1) => this.stack -= amount;
}

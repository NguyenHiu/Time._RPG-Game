using System;

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

    public void AddStack(int amount = 1) => this.stack += amount;
    public void RemoveStack(int amount = 1) => this.stack -= amount;
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItemController : DropItemController
{
    [Header("Player's Drop")]
    [Range(0f, 1f)]
    [SerializeField] private float chanceToLoseEquipments;
    [Range(0f, 1f)]
    [SerializeField] private float chanceToLoseMaterials;
    [Range(0f, 1f)]
    [SerializeField] private float chanceToLoseInventoryItems;

    public override void RandomDroppedItems()
    {
        Inventory inventory = Inventory.instance;

        // Get dropped items
        List<InventoryItem> droppedEquipments = new();
        foreach (InventoryItem item in inventory.GetEquipmentItems())
        {
            if (Random.Range(0, 100) < chanceToLoseEquipments * 100)
                droppedEquipments.Add(item);
        }

        List<InventoryItem> droppedMaterials = new();
        foreach (InventoryItem item in inventory.GetStashItems())
        {
            InventoryItem droppedItem = CalculateNumberOfLostItem(chanceToLoseMaterials * 100, item);
            if (droppedItem != null)
                droppedMaterials.Add(droppedItem);
        }

        List<InventoryItem> droppedInventoryItems = new();
        foreach (InventoryItem item in inventory.GetInventoryItems())
        {
            InventoryItem droppedItem = CalculateNumberOfLostItem(chanceToLoseInventoryItems * 100, item);
            if (droppedItem != null)
                droppedInventoryItems.Add(droppedItem);
        }

        // Drop these items
        foreach (InventoryItem item in droppedEquipments)
        {
            inventory.Unequip(item.data as EquipmentItemData);
            DropItem(item.data);
        }
        foreach (InventoryItem item in droppedMaterials)
        {
            inventory.RemoveItem(item.data, item.stack);
            DropItem(item.data);
        }
        foreach (InventoryItem item in droppedInventoryItems)
        {
            inventory.RemoveItem(item.data, item.stack);
            DropItem(item.data);
        }
    }

    private InventoryItem CalculateNumberOfLostItem(float rate, InventoryItem lostItem)
    {
        InventoryItem res = null;
        int numberOfLostItems = 0;
        for (int i = 0; i < lostItem.stack; i++)
        {
            if (Random.Range(0, 100) < rate)
                numberOfLostItems++;
        }

        // Create new InventoryItem with custom lostItems value
        if (numberOfLostItems != 0)
            res = new(lostItem.data) { stack = numberOfLostItems };

        Debug.Log(" >> Lost Item: " + numberOfLostItems);

        return res;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private Dictionary<ItemData, InventoryItem> inventoryDict;

    private void Awake()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();
    }

    private void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else instance = this;
    }

    public void AddItem(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();
        }
        else
        {
            inventoryItem = new InventoryItem(item);
            inventoryItems.Add(inventoryItem);
            inventoryDict.Add(item, inventoryItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem inventoryItem))
        {
            if (inventoryItem.stack == 1)
            {
                inventoryItems.Remove(inventoryItem);
                inventoryDict.Remove(inventoryItem.item);
            } else 
                inventoryItem.RemoveStack();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            InventoryItem lastItem = inventoryItems[^1];
            RemoveItem(lastItem.item);
        }
    }
}

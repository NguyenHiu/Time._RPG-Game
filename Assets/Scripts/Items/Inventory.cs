using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private Dictionary<ItemData, InventoryItem> inventoryDict;
    [SerializeField] private List<InventoryItem> stashItems;
    [SerializeField] private Dictionary<ItemData, InventoryItem> stashDict;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventorySlotParent;
    private UI_InventorySlot[] inventorySlots;
    [SerializeField] private GameObject stashSlotParent;
    private UI_InventorySlot[] stashSlots;

    private void Awake()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();
        stashItems = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();
    }

    private void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else instance = this;

        inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_InventorySlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_InventorySlot>();
    }

    private void UpdateInventory()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventorySlots[i].UpdateInventorySlot(inventoryItems[i]);
        }
        for (int i = 0; i < stashItems.Count; i++)
        {
            stashSlots[i].UpdateInventorySlot(stashItems[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
            AddToInventory(item);
        else if (item.itemType == ItemType.Material)
            AddToStash(item);

        UpdateInventory();
    }

    private void AddToInventory(ItemData item)
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

    private void AddToStash(ItemData item)
    {
        if (stashDict.TryGetValue(item, out InventoryItem stashItem))
        {
            stashItem.AddStack();
        }
        else
        {
            stashItem = new InventoryItem(item);
            stashItems.Add(stashItem);
            stashDict.Add(item, stashItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
            RemoveInventoryItem(item);
        else if (item.itemType == ItemType.Material)
            RemoveStashItem(item);

        UpdateInventory();
    }

    private void RemoveInventoryItem(ItemData item)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem inventoryItem))
        {
            if (inventoryItem.stack == 1)
            {
                inventoryItems.Remove(inventoryItem);
                inventoryDict.Remove(inventoryItem.data);
            }
            else
                inventoryItem.RemoveStack();
        }
    }

    private void RemoveStashItem(ItemData item)
    {
        if (stashDict.TryGetValue(item, out InventoryItem stashItem))
        {
            if (stashItem.stack == 1)
            {
                stashItems.Remove(stashItem);
                stashDict.Remove(stashItem.data);
            }
            else
                stashItem.RemoveStack();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            InventoryItem lastItem = inventoryItems[^1];
            RemoveItem(lastItem.data);
        }
    }
}

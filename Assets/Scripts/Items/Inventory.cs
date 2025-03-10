using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private Dictionary<ItemData, InventoryItem> inventoryDict;
    [SerializeField] private List<InventoryItem> stashItems;
    [SerializeField] private Dictionary<ItemData, InventoryItem> stashDict;
    [SerializeField] private List<InventoryItem> equippedItems;
    [SerializeField] private Dictionary<EquipmentItemData, InventoryItem> equippedDict;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventorySlotParent;
    private UI_InventorySlot[] inventorySlots;
    [SerializeField] private GameObject stashSlotParent;
    private UI_InventorySlot[] stashSlots;
    [SerializeField] private GameObject equipmentSlotParent;
    private UI_EquipmentSlot[] equipmentSlots;

    private void Awake()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();
        stashItems = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();
        equippedItems = new List<InventoryItem>();
        equippedDict = new Dictionary<EquipmentItemData, InventoryItem>();
    }

    private void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else instance = this;

        inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_InventorySlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_InventorySlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    public void EquipItem(ItemData _newItem)
    {
        EquipmentItemData _newEquipmentItemData = _newItem as EquipmentItemData;
        InventoryItem existingValue = null;
        EquipmentItemData existingKey = null;
        foreach (KeyValuePair<EquipmentItemData, InventoryItem> item in equippedDict)
        {
            if (item.Key.equipmentType == _newEquipmentItemData.equipmentType)
            {
                existingKey = item.Key;
                existingValue = item.Value;
                break;
            }
        }

        if (existingValue != null)
            Unequip(existingValue, existingKey);

        _newEquipmentItemData.AddModifier();
        InventoryItem newEquipItem = new(_newEquipmentItemData);
        equippedItems.Add(newEquipItem);
        equippedDict.Add(_newEquipmentItemData, newEquipItem);

        RemoveItem(_newItem);
    }

    private void Unequip(InventoryItem existingValue, EquipmentItemData existingKey)
    {
        existingValue.data.RemoveModifier();
        AddItem(existingValue.data);
        equippedItems.Remove(existingValue);
        equippedDict.Remove(existingKey);
    }

    private void UpdateInventory()
    {
        foreach(UI_EquipmentSlot slot in equipmentSlots)
        {
            bool flag = false;
            foreach(KeyValuePair<EquipmentItemData, InventoryItem> item in equippedDict)
            {
                if (item.Key.equipmentType == slot.slotType)
                {
                    slot.UpdateInventorySlot(item.Value);
                    flag = true;
                    break;
                } 
            }
            if (!flag) slot.ClearSlot();
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventoryItems.Count)
                inventorySlots[i].UpdateInventorySlot(inventoryItems[i]);
            else
                inventorySlots[i].ClearSlot();
        }

        for (int i = 0; i < stashSlots.Length; i++)
        {
            if (i < stashItems.Count)
                stashSlots[i].UpdateInventorySlot(stashItems[i]);
            else
                stashSlots[i].ClearSlot();
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

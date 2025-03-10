using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
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
        ItemData existingData = null;
        foreach (KeyValuePair<EquipmentItemData, InventoryItem> item in equippedDict)
        {
            if (item.Key.equipmentType == _newEquipmentItemData.equipmentType)
            {
                existingData = item.Key;
                break;
            }
        }

        if (existingData != null)
        {
            Unequip(existingData as EquipmentItemData);
            AddItem(existingData);
        }

        _newEquipmentItemData.AddModifier();
        InventoryItem newEquipItem = new(_newEquipmentItemData);
        equippedItems.Add(newEquipItem);
        equippedDict.Add(_newEquipmentItemData, newEquipItem);

        RemoveItem(_newItem);
    }

    public void Unequip(EquipmentItemData existingData)
    {
        if (equippedDict.TryGetValue(existingData, out InventoryItem value))
        {
            value.data.RemoveModifier();
            equippedItems.Remove(value);
            equippedDict.Remove(existingData);
        }
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

    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (item.itemType == ItemType.Equipment)
            RemoveInventoryItem(item, amount);
        else if (item.itemType == ItemType.Material)
            RemoveStashItem(item, amount);

        UpdateInventory();
    }

    private void RemoveInventoryItem(ItemData item, int amount = 1)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem inventoryItem))
        {
            if (inventoryItem.stack == amount)
            {
                inventoryItems.Remove(inventoryItem);
                inventoryDict.Remove(inventoryItem.data);
            }
            else
                inventoryItem.RemoveStack(amount);
        }
    }

    private void RemoveStashItem(ItemData item, int amount = 1)
    {
        if (stashDict.TryGetValue(item, out InventoryItem stashItem))
        {
            if (stashItem.stack == amount)
            {
                stashItems.Remove(stashItem);
                stashDict.Remove(stashItem.data);
            }
            else
                stashItem.RemoveStack(amount);
        }
    }

    public bool Craft(EquipmentItemData _itemToCraft)
    {
        List<InventoryItem> requiredMaterials = _itemToCraft.requiredMaterials;
        foreach (InventoryItem item in requiredMaterials)
        {
            if (stashDict.TryGetValue(item.data, out InventoryItem inventoryItem))
            {
                if (inventoryItem.stack < item.stack)
                    return false;
            }
            else return false;
        }

        foreach (InventoryItem item in requiredMaterials)
        {
            if (stashDict.TryGetValue(item.data, out InventoryItem inventoryItem))
            {
                if (inventoryItem.stack < item.stack)
                    return false;

                inventoryItem.RemoveStack(item.stack);
            }
            else return false;
        }

        AddItem(_itemToCraft);

        return true;
    }

    public List<InventoryItem> GetEquipmentItems() => equippedItems;
    public List<InventoryItem> GetStashItems() => stashItems;
    public List<InventoryItem> GetInventoryItems() => inventoryItems;
}

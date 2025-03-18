using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct StartedItem
{
    public ItemData item;
    public int stack;
}

public class Inventory : MonoBehaviour, IGameData
{
    public static Inventory instance;

    [Header("Started Pack")]
    [SerializeField] private List<StartedItem> startedPack;

    [Header("Inventory")]
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
    [SerializeField] private GameObject statSlotsParent;
    private UI_StatSlot[] statSlots;

    private float lastTimeUsedFlask = 0;
    private float flaskCooldown = 0;
    public Action<float> OnFlaskCooldownUpdated;

    private float lastTimeUsedArmor = 0;
    private float armorCooldown = 0;

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
        statSlots = statSlotsParent.GetComponentsInChildren<UI_StatSlot>();

        foreach (StartedItem item in startedPack)
            AddItem(item.item, item.stack);
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
            Debug.Log(">> I THINK SOMETHIGN ISBROKEN");
            Unequip(existingData as EquipmentItemData);
            AddItem(existingData);
        }

        _newEquipmentItemData.AddModifier();
        InventoryItem newEquipItem = new(_newEquipmentItemData);
        equippedItems.Add(newEquipItem);
        equippedDict.Add(_newEquipmentItemData, newEquipItem);

        UpdateInventory();
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
        foreach (UI_EquipmentSlot slot in equipmentSlots)
        {
            bool flag = false;
            foreach (KeyValuePair<EquipmentItemData, InventoryItem> item in equippedDict)
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

        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValue();
        }
    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item.itemType == ItemType.Equipment && !IsInventoryFull())
            AddToInventory(item, amount);
        else if (item.itemType == ItemType.Material)
            AddToStash(item, amount);

        UpdateInventory();
    }

    private void AddToInventory(ItemData item, int amount = 1)
    {
        if (inventoryDict.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack(amount);
        }
        else
        {
            Debug.Log("AddToInventory, name: " + item.name + "; value: " + amount);
            inventoryItem = new InventoryItem(item);
            inventoryItem.stack = amount;
            inventoryItems.Add(inventoryItem);
            inventoryDict.Add(item, inventoryItem);
        }
    }

    private void AddToStash(ItemData item, int amount = 1)
    {
        if (stashDict.TryGetValue(item, out InventoryItem stashItem))
        {
            stashItem.AddStack(amount);
        }
        else
        {
            stashItem = new InventoryItem(item);
            stashItem.stack = amount;
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

                RemoveItem(inventoryItem.data, item.stack);
            }
            else return false;
        }

        AddItem(_itemToCraft);

        Debug.Log("Craft successfully!");

        return true;
    }

    public List<InventoryItem> GetEquipmentItems() => equippedItems;
    public List<InventoryItem> GetStashItems() => stashItems;
    public List<InventoryItem> GetInventoryItems() => inventoryItems;
    public EquipmentItemData GetEquipmentByType(EquipmentType _type)
    {
        foreach (InventoryItem item in equippedItems)
        {
            if (((EquipmentItemData)item.data).equipmentType == _type)
                return item.data as EquipmentItemData;
        }

        return null;
    }

    public void TryUseFlask()
    {
        foreach (KeyValuePair<EquipmentItemData, InventoryItem> item in equippedDict)
        {
            if (item.Key.equipmentType == EquipmentType.Flask)
            {
                if (Time.time > (lastTimeUsedFlask + flaskCooldown))
                {
                    flaskCooldown = item.Key.itemCooldown;
                    lastTimeUsedFlask = Time.time;
                    OnFlaskCooldownUpdated?.Invoke(flaskCooldown);
                    item.Key.ExecuteEffects(null);
                }
            }
        }
    }

    public void TryUseArmor()
    {
        foreach (KeyValuePair<EquipmentItemData, InventoryItem> item in equippedDict)
        {
            if (item.Key.equipmentType == EquipmentType.Armor)
            {
                if (Time.time > (lastTimeUsedArmor + armorCooldown))
                {
                    armorCooldown = item.Key.itemCooldown;
                    lastTimeUsedArmor = Time.time;
                    item.Key.ExecuteEffects(PlayerManager.instance.player.transform);
                }
            }
        }
    }

    public bool IsInventoryFull()
    {
        return inventoryItems.Count >= inventorySlots.Length;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.inventory.Clear();

        foreach (InventoryItem item in inventoryItems)
            gameData.inventory.Add(item.data.itemID, item.stack);

        foreach (InventoryItem item in stashItems)
            gameData.inventory.Add(item.data.itemID, item.stack);

        gameData.equipments.Clear();
        foreach (InventoryItem item in equippedItems)
            gameData.equipments.Add(item.data.itemID);
    }

    public void LoadData(GameData gameData)
    {
        Dictionary<String, ItemData> assetDict = new();
        string[] assetIDs = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });
        foreach (string id in assetIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(id);
            ItemData asset = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
            assetDict.Add(id, asset);
        }

        foreach (KeyValuePair<String, int> item in gameData.inventory)
        {
            if (!assetDict.ContainsKey(item.Key))
            {
                Debug.LogError("Invalid Item ID: " + item.Key);
                return;
            }

            AddItem(assetDict[item.Key], item.Value);
        }

        foreach (string id in gameData.equipments)
        {
            if (!assetDict.ContainsKey(id))
            {
                Debug.LogError("Invalid Item ID: " + id);
                return;
            }

            EquipItem(assetDict[id]);
        }
    }
}

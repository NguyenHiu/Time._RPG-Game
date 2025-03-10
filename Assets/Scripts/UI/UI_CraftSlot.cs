using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_InventorySlot
{
    private void OnEnable()
    {
        UpdateInventorySlot(itemData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        bool isSuccessful = Inventory.instance.Craft(itemData.data as EquipmentItemData);
        if (!isSuccessful) 
            Debug.Log("Don't have enough materials");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentSlot : UI_InventorySlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }
}

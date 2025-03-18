using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_InventorySlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (
            itemData != null &&
            itemData.data != null &&
            itemData.data.itemType == ItemType.Equipment
        )
        {
            Inventory.instance.Unequip(itemData.data as EquipmentItemData);
            Inventory.instance.AddItem(itemData.data as EquipmentItemData);
            ui.equipmentTooltips.DisableTooltips();
        }
    }
}

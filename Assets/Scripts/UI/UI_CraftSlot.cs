using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_InventorySlot
{
    private void OnEnable()
    {
        UpdateInventorySlot(itemData);
    }

    public void SetupCraftSlot(InventoryItem invenItem)
    {
        itemData = invenItem;
        itemImage.sprite = invenItem.data.icon;
        itemText.text = invenItem.data.itemName;

        if (itemText.text.Length > 14)
            itemText.fontSize *= .7f;
        else itemText.fontSize = 20;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.UpdateCraftItem(itemData.data as EquipmentItemData);
    }
}

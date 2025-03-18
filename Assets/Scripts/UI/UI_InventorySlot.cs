using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UIImage = UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected UIImage.Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    public InventoryItem itemData;
    protected UI ui;

    protected void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemData != null && itemData.data != null)
        {
            if (Input.GetKey(KeyCode.LeftControl))
                Inventory.instance.RemoveItem(itemData.data);
            else if (itemData.data.itemType == ItemType.Equipment)
            {
                Inventory.instance.EquipItem(itemData.data);
                Inventory.instance.RemoveItem(itemData.data);
            }

            ui.equipmentTooltips.DisableTooltips();
        }
    }

    public void UpdateInventorySlot(InventoryItem _newItem)
    {
        itemData = _newItem;
        itemImage.color = Color.white;

        if (itemData != null)
        {
            itemImage.sprite = itemData.data.icon;

            if (itemData.stack > 1)
                itemText.text = itemData.stack.ToString();
            else itemText.text = "";
        }
        else
        {
            Debug.Log("itemData is null");
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemText.text = "";
        }

    }

    public void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
        itemData = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (
            itemData != null && 
            itemData.data != null &&
            itemData.data.itemType == ItemType.Equipment
        )
            ui.equipmentTooltips.EnableTooltips(itemData.data as EquipmentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemData != null)
            ui.equipmentTooltips.DisableTooltips();
    }
}

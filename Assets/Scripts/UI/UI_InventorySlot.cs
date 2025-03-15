using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UIImage = UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UIImage.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    public InventoryItem itemData;
    private UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl) && itemData != null)
        {
            Inventory.instance.RemoveItem(itemData.data);
            return;
        }

        if (itemData != null && itemData.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(itemData.data);
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
        if (itemData != null)
            ui.equipmentTooltips.EnableTooltips(itemData.data as EquipmentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemData != null)
            ui.equipmentTooltips.DisableTooltips();
    }
}

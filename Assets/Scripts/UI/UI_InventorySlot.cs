using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InventorySlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    public InventoryItem itemData;

    public void OnPointerDown(PointerEventData eventData)
    {
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
        } else
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
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    public InventoryItem itemData;

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
}

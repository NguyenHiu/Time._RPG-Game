using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    private InventoryItem itemData;

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
}

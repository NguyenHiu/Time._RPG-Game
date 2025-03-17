using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image[] materialImages; // to be assigned manually
    [SerializeField] private Button craftButton;

    public void UpdateCraftItem(EquipmentItemData item)
    {
        craftButton.onClick.RemoveAllListeners();

        ClearCurrentItem();
        icon.sprite = item.icon;
        itemName.text = item.itemName;
        itemDescription.text = item.GetDescription();

        if (item.requiredMaterials.Count > materialImages.Length)
            Debug.Log("This item requires more material slots!!!");

        for (int i = 0; i < item.requiredMaterials.Count; i++)
        {
            materialImages[i].sprite = item.requiredMaterials[i].data.icon;
            materialImages[i].color = Color.white;

            TextMeshProUGUI amount = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
            amount.text = item.requiredMaterials[i].stack.ToString();
            amount.color = Color.white;
        }

        craftButton.onClick.AddListener(() => Inventory.instance.Craft(item));
    }

    private void ClearCurrentItem()
    {
        // Hide all materials
        foreach (Image img in materialImages)
        {
            img.color = Color.clear;
            img.GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
    }
}

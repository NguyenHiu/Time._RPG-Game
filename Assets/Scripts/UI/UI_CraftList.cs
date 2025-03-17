using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private List<ItemData> items;
    [SerializeField] private Transform craftSlotsParent;
    [SerializeField] private GameObject craftSlotPrefab;

    private void Start()
    {
        InitCraftList();
    }

    public void InitCraftList()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().CreateCraftList();
        if (items.Count > 0 && items[0] != null)
            GetComponentInParent<UI>().craftWindow.UpdateCraftItem(items[0] as EquipmentItemData);
    }

    private void CreateCraftList()
    {
        ClearCraftList();
        foreach (ItemData item in items)
        {
            GameObject slotObj = Instantiate(craftSlotPrefab, craftSlotsParent);
            slotObj.GetComponent<UI_CraftSlot>().SetupCraftSlot(new(item));
        }
    }

    private void ClearCraftList()
    {
        for (int i = 0; i < craftSlotsParent.childCount; i++)
        {
            Destroy(craftSlotsParent.GetChild(i).gameObject);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        CreateCraftList();
    }

}

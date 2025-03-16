using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftList : MonoBehaviour, IPointerDownHandler
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
        Debug.Log("Init Craft List");
        transform.parent.GetChild(0).GetComponent<CraftList>().CreateCraftList();
        if (items[0] != null)
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

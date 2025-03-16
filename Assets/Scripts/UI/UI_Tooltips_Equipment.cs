using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltips_Equipment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI description;

    public void EnableTooltips(EquipmentItemData item)
    {
        title.text = item.name;
        type.text = item.equipmentType.ToString();
        description.text = item.GetDescription();

        gameObject.SetActive(true);
    }

    public void DisableTooltips()
    {
        gameObject.SetActive(false);
    }
}

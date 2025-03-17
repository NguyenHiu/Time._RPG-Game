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

        CorrectPosition();
        gameObject.SetActive(true);
    }

    public void DisableTooltips()
    {
        gameObject.SetActive(false);
    }

    private void CorrectPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 offset = new(150, 150);
        if (mousePos.x > Screen.width / 2) offset.x *= -1;
        if (mousePos.y > Screen.height / 2) offset.y *= -1;
        transform.position = mousePos + offset;
    }
}

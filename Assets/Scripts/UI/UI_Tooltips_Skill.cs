using TMPro;
using UnityEngine;

public class UI_Tooltips_Skill : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillDescription;
    [SerializeField] TextMeshProUGUI skillCost;

    private void Start()
    {
        DisableTooltips();
    }

    public void EnableTooltips(UI_SkillSlot skillSlot)
    {
        if (skillSlot != null)
        {
            skillName.text = skillSlot.skillName;
            skillDescription.text = skillSlot.skillDescription;
            skillCost.text = "Cost: " + skillSlot.skillPrice;
            CorrectPosition();
            gameObject.SetActive(true);
        }
    }

    private void CorrectPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 offset = new(150, 150);
        if (mousePos.x > Screen.width / 2) offset.x *= -1;
        if (mousePos.y > Screen.height / 2) offset.y *= -1;
        transform.position = mousePos + offset;
    }

    public void DisableTooltips()
    {
        gameObject.SetActive(false);
    }
}

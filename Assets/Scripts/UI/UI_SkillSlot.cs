using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image skillImage;
    private UI ui;

    public string skillName;
    [TextArea]
    public string skillDescription;
    public int skillPrice;

    public bool isLocked = true;
    [SerializeField] private List<UI_SkillSlot> shouldBeUnlocked;
    [SerializeField] private List<UI_SkillSlot> shouldBeLocked;

    [SerializeField] private Color lockedColor;

    private void OnValidate()
    {
        gameObject.name = "SkillSlot - " + skillName;
    }

    private void Awake()
    {
        skillImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    private void Start()
    {
        skillImage.color = lockedColor;
        ui = GetComponentInParent<UI>();
    }

    private void UnlockSkill()
    {
        if (!isLocked) return;

        foreach (UI_SkillSlot slot in shouldBeLocked)
        {
            if (!slot.isLocked)
            {
                Debug.Log("Skill [" + slot.skillName + "] should be locked");
                return;
            }
        }
        foreach (UI_SkillSlot slot in shouldBeUnlocked)
        {
            if (slot.isLocked)
            {
                Debug.Log("Skill [" + slot.skillName + "] should be unlocked");
                return;
            }
        }

        if (!PlayerManager.instance.SpendCurrency(skillPrice))
            return;

        isLocked = false;
        skillImage.color = Color.white;
        Debug.Log("Unlock skill successfully");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltips.EnableTooltips(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltips.DisableTooltips();
    }
}

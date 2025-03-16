using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image skillImage;
    private UI ui;

    [SerializeField] public string skillName;
    [TextArea]
    [SerializeField] public string skillDescription;

    [SerializeField] private bool isLocked = true;
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
    }

    private void Start()
    {
        skillImage.color = lockedColor;
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
        ui = GetComponentInParent<UI>();
    }

    public void UnlockSkill()
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

        Debug.Log("Unlock skill successfully");
        isLocked = false;
        skillImage.color = Color.white;
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;
    private StatsController statCtrl;
    private UI ui;

    [TextArea]
    public string description = "";

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        statNameText.text = statName;
        UpdateStatValue();
    }

    public void UpdateStatValue()
    {
        if (statCtrl == null)
            statCtrl = PlayerManager.instance.player.statCtrl;

        switch (statType)
        {
            case StatType.maxHP:
                statValueText.text = statCtrl.GetTotalMaxHealth().ToString();
                break;
            case StatType.damage:
                statValueText.text = (statCtrl.damage.GetValue() + statCtrl.strength.GetValue()).ToString();
                break;
            case StatType.critPower:
                statValueText.text = (statCtrl.critPower.GetValue() + statCtrl.strength.GetValue()).ToString();
                break;
            case StatType.critChance:
                statValueText.text = (statCtrl.critChance.GetValue() + statCtrl.agility.GetValue()).ToString();
                break;
            case StatType.fireDamage:
                statValueText.text = (statCtrl.fireDamage.GetValue() + statCtrl.intelligent.GetValue()).ToString();
                break;
            case StatType.iceDamage:
                statValueText.text = (statCtrl.iceDamage.GetValue() + statCtrl.intelligent.GetValue()).ToString();
                break;
            case StatType.lightingDamage:
                statValueText.text = (statCtrl.lightingDamage.GetValue() + statCtrl.intelligent.GetValue()).ToString();
                break;
            case StatType.magicResistance:
                statValueText.text = (statCtrl.magicResistance.GetValue() + statCtrl.intelligent.GetValue() * 3).ToString();
                break;
            default:
                statValueText.text = statCtrl.GetStat(statType).GetValue().ToString();
                break;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltips.EnableTooltips(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltips.DisableTooltips();
    }
}

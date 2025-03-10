using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "Create New Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    [Header("Major ints")]
    public float strength;    // 1 point --> +1 damage; +1% crit.damage
    public float agility;     // 1 point --> +1 evasion; +1% crit.chance
    public float intelligent; // 1 point --> +1 magic damage; +3% magic resistance
    public float vitality;    // 1 point --> +3 or +5 heath points

    [Header("Defensive ints")]
    public float maxHP;
    public float armor;
    public float evasion;

    [Header("Offensive ints")]
    public float damage;
    public float critChance; // percent
    public float critPower; // e.g. 150 --> 150% * total_damage

    [Header("Magic ints")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Craft Materials")]
    public List<InventoryItem> requiredMaterials;

    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    public void AddModifier()
    {
        Player player = PlayerManager.instance.player;
        player.statCtrl.strength.AddModifier(strength);
        player.statCtrl.agility.AddModifier(agility);
        player.statCtrl.intelligent.AddModifier(intelligent);
        player.statCtrl.vitality.AddModifier(vitality);
        player.statCtrl.maxHP.AddModifier(maxHP);
        player.statCtrl.armor.AddModifier(armor);
        player.statCtrl.evasion.AddModifier(evasion);
        player.statCtrl.damage.AddModifier(damage);
        player.statCtrl.critChance.AddModifier(critChance);
        player.statCtrl.critPower.AddModifier(critPower);
        player.statCtrl.fireDamage.AddModifier(fireDamage);
        player.statCtrl.iceDamage.AddModifier(iceDamage);
        player.statCtrl.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifier()
    {
        Player player = PlayerManager.instance.player;
        player.statCtrl.strength.RemoveModifier(strength);
        player.statCtrl.agility.RemoveModifier(agility);
        player.statCtrl.intelligent.RemoveModifier(intelligent);
        player.statCtrl.vitality.RemoveModifier(vitality);
        player.statCtrl.maxHP.RemoveModifier(maxHP);
        player.statCtrl.armor.RemoveModifier(armor);
        player.statCtrl.evasion.RemoveModifier(evasion);
        player.statCtrl.damage.RemoveModifier(damage);
        player.statCtrl.critChance.RemoveModifier(critChance);
        player.statCtrl.critPower.RemoveModifier(critPower);
        player.statCtrl.fireDamage.RemoveModifier(fireDamage);
        player.statCtrl.iceDamage.RemoveModifier(iceDamage);
        player.statCtrl.lightningDamage.RemoveModifier(lightningDamage);

    }
}

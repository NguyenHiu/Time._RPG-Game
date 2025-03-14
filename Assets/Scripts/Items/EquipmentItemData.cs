using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "Create New Item", menuName = "Data/Equipment")]
public class EquipmentItemData : ItemData
{
    public EquipmentType equipmentType;
    public float itemCooldown;

    [Header("Item Effects")]
    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public float strength;    // 1 point --> +1 damage; +1% crit.damage
    public float agility;     // 1 point --> +1 evasion; +1% crit.chance
    public float intelligent; // 1 point --> +1 magic damage; +3% magic resistance
    public float vitality;    // 1 point --> +3 or +5 heath points

    [Header("Defensive Stats")]
    public float maxHP;
    public float armor;
    public float evasion;

    [Header("Offensive Stats")]
    public float damage;
    public float critChance; // percent
    public float critPower; // e.g. 150 --> 150% * total_damage

    [Header("Magic Stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Craft Materials")]
    public List<InventoryItem> requiredMaterials;


    public override void AddModifier()
    {
        Player player = PlayerManager.instance.player;
        if (strength != 0) 
            player.statCtrl.strength.AddModifier(strength);
        if (agility != 0) 
            player.statCtrl.agility.AddModifier(agility);
        if (intelligent != 0) 
            player.statCtrl.intelligent.AddModifier(intelligent);
        if (vitality != 0) 
            player.statCtrl.vitality.AddModifier(vitality);
        if (maxHP != 0) 
            player.statCtrl.maxHP.AddModifier(maxHP);
        if (armor != 0) 
            player.statCtrl.armor.AddModifier(armor);
        if (evasion != 0) 
            player.statCtrl.evasion.AddModifier(evasion);
        if (damage != 0) 
            player.statCtrl.damage.AddModifier(damage);
        if (critChance != 0) 
            player.statCtrl.critChance.AddModifier(critChance);
        if (critPower != 0) 
            player.statCtrl.critPower.AddModifier(critPower);
        if (fireDamage != 0) 
            player.statCtrl.fireDamage.AddModifier(fireDamage);
        if (iceDamage != 0) 
            player.statCtrl.iceDamage.AddModifier(iceDamage);
        if (lightningDamage != 0) 
            player.statCtrl.lightningDamage.AddModifier(lightningDamage);
    }

    public override void RemoveModifier()
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
    public virtual void ExecuteEffects(Transform _enemyTrans)
    {
        foreach (ItemEffect ie in itemEffects)
        {
            ie.ExecuteEffect(_enemyTrans);
        }
    }
}

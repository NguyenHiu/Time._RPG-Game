using System.Collections;
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
}

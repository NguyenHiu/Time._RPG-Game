using System.Text;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "Create New Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    protected StringBuilder sb = new();

    public virtual string GetDescription() => "";

    public virtual void AddModifier() { }

    public virtual void RemoveModifier() { }

}

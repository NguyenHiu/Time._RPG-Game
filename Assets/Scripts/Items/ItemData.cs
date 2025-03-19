using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public string itemID;

    protected StringBuilder sb = new();

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(itemID))
        {
            string path = AssetDatabase.GetAssetPath(this);
            itemID = AssetDatabase.AssetPathToGUID(path);
            EditorUtility.SetDirty(this);
        }
#endif
    }

    public virtual string GetDescription() => "";

    public virtual void AddModifier() { }

    public virtual void RemoveModifier() { }

}

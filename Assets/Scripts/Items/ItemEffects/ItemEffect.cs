using UnityEngine;

[CreateAssetMenu(fileName = "Item Effect", menuName = "Data/Item Effects")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _transform)
    {
        Debug.Log("Executing item effect...");
    }
}

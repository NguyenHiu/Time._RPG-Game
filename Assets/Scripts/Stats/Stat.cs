using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<float> modifers = new();

    public float GetValue()
    {
        float finalVal = baseValue;
        foreach (float val in modifers)
        {
            finalVal += val;
        }
        return finalVal;
    }

    public void SetBaseValue(float val)
    {
        baseValue = val;
    }

    public void AddModifier(float val)
    {
        modifers.Add(val);
    }

    public void RemoveModifier(float val)
    {
        modifers.Remove(val);
    }
}

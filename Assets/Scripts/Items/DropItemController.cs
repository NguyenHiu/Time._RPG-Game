using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rd = UnityEngine.Random;

[Serializable]
public struct DroppedItem
{
    public ItemData item;
    [Range(0f, 1f)] public float rate;
}

public class DropItemController : MonoBehaviour
{
    [SerializeField] private List<DroppedItem> droppedItems = new();
    [SerializeField] private GameObject dropPrefab;

    private List<int> GetListOfIndex()
    {
        List<int> res = new();
        for (int i = 0; i < droppedItems.Count; i++)
            res.Add(i);
        return res;
    }

    public virtual void RandomDroppedItems()
    {
        List<int> rdIdx = GetListOfIndex();
        while (rdIdx.Count > 0)
        {
            int idx = rdIdx[rd.Range(0, rdIdx.Count)];
            rdIdx.Remove(idx);
            if (rd.Range(0, 100) < droppedItems[idx].rate * 100)
                DropItem(droppedItems[idx].item);
        }
    }

    protected void DropItem(ItemData _item)
    {
        GameObject droppedObj = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new(rd.Range(-5, 5), rd.Range(15, 20));
        droppedObj.GetComponent<ItemObject>().SetupItem(_item, randomVelocity);
    }
}

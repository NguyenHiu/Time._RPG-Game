using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effects/Thunder Strike Effect")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _transform)
    {
        GameObject thunderStrike = Instantiate(thunderStrikePrefab, _transform.position, Quaternion.identity);
        Destroy(thunderStrike, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Effect", menuName = "Data/Item Effects/Healing")]
public class Healing : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healthPercent;

    public override void ExecuteEffect(Transform _transform)
    {
        StatsController pStatCtrl = PlayerManager.instance.player.statCtrl;
        int playerMaxHealth = pStatCtrl.GetTotalMaxHealth();
        pStatCtrl.IncreaseHealth(Mathf.RoundToInt(healthPercent * playerMaxHealth));
    }
}

using UnityEngine;


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effects/Buff")]
public class Buff : ItemEffect
{
    [SerializeField] private StatType type;
    [SerializeField] private float amount;
    [SerializeField] private float duration;
    private StatsController stats;

    public override void ExecuteEffect(Transform _enemyTrans)
    {
        stats = PlayerManager.instance.player.statCtrl;

        Stat buffStat = stats.GetStat(type);
        if (buffStat != null)
            stats.Buff(buffStat, amount, duration);
    }
}

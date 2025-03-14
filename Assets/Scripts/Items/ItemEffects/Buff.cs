using UnityEngine;

public enum BuffType
{
    strength,
    agility,
    intelligent,
    vitality,
    maxHP,
    armor,
    evasion,
    damage,
    critChance,
    critPower,
    fireDamage,
    iceDamage,
    lightningDamage
}

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effects/Buff")]
public class Buff : ItemEffect
{
    [SerializeField] private BuffType type;
    [SerializeField] private float amount;
    [SerializeField] private float duration;
    private StatsController stats;

    public override void ExecuteEffect(Transform _enemyTrans)
    {
        stats = PlayerManager.instance.player.statCtrl;

        Stat buffStat = GetStatType(type);
        if (buffStat != null)
            stats.Buff(buffStat, amount, duration);
    }

    private Stat GetStatType(BuffType type)
    {
        switch (type)
        {
            case BuffType.strength:
                return stats.strength;
            case BuffType.agility:
                return stats.agility;
            case BuffType.intelligent:
                return stats.intelligent;
            case BuffType.vitality:
                return stats.vitality;
            case BuffType.maxHP:
                return stats.maxHP;
            case BuffType.armor:
                return stats.armor;
            case BuffType.evasion:
                return stats.evasion;
            case BuffType.damage:
                return stats.damage;
            case BuffType.critChance:
                return stats.critChance;
            case BuffType.critPower:
                return stats.critPower;
            case BuffType.fireDamage:
                return stats.fireDamage;
            case BuffType.iceDamage:
                return stats.iceDamage;
            case BuffType.lightningDamage:
                return stats.lightningDamage;
            default: break;
        }
        return null;
    }
}

using UnityEngine;

public class EnemyStats : StatsController
{
    private Enemy enemy;

    [Header("Dropped Items")]
    private DropItemController dropCtrl;

    [Header("Level")]
    [SerializeField] private int level = 1;
    [Range(0f, 1f)]
    [SerializeField] private float statsIncreasePercent = .4f;

    protected override void Start()
    {
        ApplyLevelModifer();
        base.Start();
        enemy = GetComponent<Enemy>();
        dropCtrl = GetComponent<DropItemController>();
    }

    protected virtual void ApplyLevelModifer()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligent);
        Modify(vitality);
        Modify(maxHP);
        Modify(armor);
        Modify(evasion);
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    protected virtual void Modify(Stat _stat)
    {
        for (int i = 1; i <= level; i++)
        {
            float modiferVal = _stat.GetValue() * statsIncreasePercent;
            _stat.AddModifier(modiferVal);
        }
    }

    public override void TakeDamage(int damage, bool triggerAffect = true)
    {
        base.TakeDamage(damage, triggerAffect);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        dropCtrl.RandomDroppedItems();
    }

    public override void DoDamage(StatsController ctrl)
    {
        base.DoDamage(ctrl);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsController
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }

    public override void DoDamage(StatsController ctrl)
    {
        base.DoDamage(ctrl);
    }
}

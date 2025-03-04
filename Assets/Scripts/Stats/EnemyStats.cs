using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsController
{
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }
}

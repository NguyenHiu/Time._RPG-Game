public class EnemyStats : StatsController
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage, bool triggerAffect = true)
    {
        base.TakeDamage(damage, triggerAffect);
    }

    public void TakeMagicalDamage(int damage)
    {
        base.TakeDamage(damage);
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

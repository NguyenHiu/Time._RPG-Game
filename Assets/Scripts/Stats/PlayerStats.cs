public class PlayerStats : StatsController
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
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
        player.Die();
    }

    public override void DoDamage(StatsController ctrl)
    {
        base.DoDamage(ctrl);
    }
}

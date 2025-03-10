public class PlayerStats : StatsController
{
    private Player player;
    private PlayerDropItemController dropCtrl;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        dropCtrl = GetComponent<PlayerDropItemController>();
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
        dropCtrl.RandomDroppedItems();
    }

    public override void DoDamage(StatsController ctrl)
    {
        base.DoDamage(ctrl);
    }
}

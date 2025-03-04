using UnityEngine;

public class StatsController : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;    // 1 point --> +1 damage; +1% crit.damage
    public Stat agility;     // 1 point --> +1 evasion; +1% crit.chance
    public Stat intelligent; // 1 point --> +1 magic damage; +1% magic resistance
    public Stat vitality;    // 1 point --> +3 or +5 heath points

    [Header("Defensive Stats")]
    public Stat maxHP;
    public Stat armor;
    public Stat evasion;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance; // percent
    public Stat critPower; // e.g. 150 --> 150% * total_damage

    [SerializeField] private float currentHp;

    protected virtual void Start()
    {
        currentHp = maxHP.GetValue();
        critPower.SetBaseValue(150); // default 150% of the total damage
    }

    public virtual void DoDamage(StatsController ctrl)
    {
        if (IsMissedAttack(ctrl)) { return; }

        float totalDamage = damage.GetValue() + strength.GetValue();

        if (IsCritical())
            totalDamage *= (critPower.GetValue() + strength.GetValue()) * .01f;

        float finalDamage = AppliedArmor(ctrl, totalDamage);

        ctrl.TakeDamage((int)finalDamage);
    }

    private bool IsMissedAttack(StatsController ctrl)
    {
        float evasionPoints = ctrl.evasion.GetValue() + ctrl.agility.GetValue();
        return (Random.Range(0, 100) < evasionPoints);
    }

    private float AppliedArmor(StatsController ctrl, float currDamage)
    {
        float finalDamage = currDamage - ctrl.armor.GetValue();
        if (finalDamage < 0)
            return 0;
        return finalDamage;
    }

    private bool IsCritical()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        return (Random.Range(0, 100) < totalCriticalChance);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died.");
    }

}

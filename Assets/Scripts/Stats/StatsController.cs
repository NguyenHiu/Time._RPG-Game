using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;    // 1 point --> +1 damage; +1% crit.damage
    public Stat agility;     // 1 point --> +1 evasion; +1% crit.chance
    public Stat intelligent; // 1 point --> +1 magic damage; +3% magic resistance
    public Stat vitality;    // 1 point --> +3 or +5 heath points

    [Header("Defensive Stats")]
    public Stat maxHP;
    public Stat armor;
    public Stat evasion;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance; // percent
    public Stat critPower; // e.g. 150 --> 150% * total_damage

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isBurned = false;
    public float burnTimer;
    public float burnDuration = .5f;

    public bool isChilled = false;
    public float chillTimer;
    public float chillDuration = 1.0f;

    public bool isShocked = false;
    public float shockTimer;
    public float shockDuration = 1.0f;

    [SerializeField] private float currentHp;

    protected virtual void Start()
    {
        currentHp = maxHP.GetValue();
        critPower.SetBaseValue(150); // default 150% of the total damage
    }

    protected virtual void Update()
    {
        if (isBurned)
        {
            burnTimer -= Time.deltaTime;
            if (burnTimer < 0)
                isBurned = false;
        }

        if (isChilled)
        {
            chillTimer -= Time.deltaTime;
            if (chillTimer < 0)
                isChilled = false;
        }

        if (isShocked)
        {
            shockTimer -= Time.deltaTime;
            if (shockTimer < 0)
                isShocked = false;
        }
    }

    // Physical Attacks
    public virtual void DoDamage(StatsController ctrl)
    {
        MagicAttack(ctrl);

        if (IsMissedAttack(ctrl)) { return; }

        float totalDamage = damage.GetValue() + strength.GetValue();

        if (IsCritical())
            totalDamage *= (critPower.GetValue() + strength.GetValue()) * .01f;

        float finalDamage = AppliedArmor(ctrl, totalDamage);
        ctrl.TakeDamage(Mathf.RoundToInt(finalDamage));
    }

    private bool IsMissedAttack(StatsController ctrl)
    {
        float evasionPoints = ctrl.evasion.GetValue() + ctrl.agility.GetValue();
        // decrease 20% accuracy
        if (isShocked)
        {
            Debug.Log("decrease 20% accuracy");
            evasionPoints *= 1.2f;
        }
        return (Random.Range(0, 100) < evasionPoints);
    }

    private float AppliedArmor(StatsController ctrl, float currDamage)
    {
        float finalDamage = currDamage;
        if (ctrl.isChilled)
        {
            // decrease 20% defense
            Debug.Log("decrease 20% defense");
            finalDamage -= ctrl.armor.GetValue() * .8f;
        }
        else
            finalDamage -= ctrl.armor.GetValue();

        if (finalDamage < 0)
            return 0;
        return finalDamage;
    }

    private bool IsCritical()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        return (Random.Range(0, 100) < totalCriticalChance);
    }

    // Magical Attacks
    private void MagicAttack(StatsController ctrl)
    {
        List<string> abilities = new();
        if (fireDamage.GetValue() > 0) abilities.Add("fire");
        if (iceDamage.GetValue() > 0) abilities.Add("ice");
        if (lightningDamage.GetValue() > 0) abilities.Add("lightning");

        string rdAffect = abilities[Random.Range(0, abilities.Count)];
        if (rdAffect == "fire")
        {
            ctrl.isBurned = true;
            ctrl.burnTimer = ctrl.burnDuration;
            ctrl.TakeDamage(Mathf.RoundToInt(fireDamage.GetValue()));
        }
        else if (rdAffect == "ice")
        {
            ctrl.isChilled = true;
            ctrl.chillTimer = ctrl.chillDuration;
            ctrl.TakeDamage(Mathf.RoundToInt(iceDamage.GetValue()));
        }
        else if (rdAffect == "lightning")
        {
            ctrl.isShocked = true;
            ctrl.shockTimer = ctrl.shockDuration;
            ctrl.TakeDamage(Mathf.RoundToInt(lightningDamage.GetValue()));
        }
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

using System.Collections;
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

    public float burnLastTime;
    public bool isBurned = false;
    public float burnTimer;
    public float burnDuration = 1.0f;

    public bool isChilled = false;
    public float chillTimer;
    public float chillDuration = 1.0f;

    public bool isShocked = false;
    public float shockTimer;
    public float shockDuration = 1.0f;

    [SerializeField] private float currentHp;
    public System.Action onHealthChanged;
    public EntityFX entityFX;
    private Entity entity;
    private bool isDeath = false;

    protected virtual void Start()
    {
        critPower.SetBaseValue(150); // default 150% of the total damage
        currentHp = GetTotalMaxHealth();
        entityFX = GetComponent<EntityFX>();
        entity = GetComponent<Entity>();
    }

    protected virtual void Update()
    {
        if (isBurned)
        {
            burnTimer -= Time.deltaTime;
            if (burnTimer < 0)
                isBurned = false;
            else if (!isDeath)
            {
                burnLastTime -= Time.deltaTime;
                if (burnLastTime < 0)
                {
                    float finalDamage = AppliedMagicResistance(this, fireDamage.GetValue());
                    TakeDamage(Mathf.RoundToInt(finalDamage), false);
                    burnLastTime = .5f;
                }
            }
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

    // GetTotalHealth returns the total health value, including max HP and vitality points
    public virtual int GetTotalMaxHealth()
    {
        return Mathf.RoundToInt(maxHP.GetValue() + vitality.GetValue() * 5);
    }

    // GetCurrentHealth returns the current health value
    public virtual int GetCurrentHealth()
    {
        return Mathf.RoundToInt(currentHp);
    }

    public virtual float GetCurrentHealthPercent()
    {
        return GetCurrentHealth() * 100 / GetTotalMaxHealth();
    }

    public virtual void Buff(Stat buffType, float amount, float duration)
    {
        StartCoroutine(BuffIn(buffType, amount, duration));
    }

    private IEnumerator BuffIn(Stat buffType, float amount, float duration)
    {
        buffType.AddModifier(amount);
        yield return new WaitForSeconds(duration);
        buffType.RemoveModifier(amount);
    }

    // Magical Attacks
    public virtual void DoFireDamage(StatsController ctrl)
    {
        // Update Fire Data
        ctrl.isBurned = true;
        ctrl.burnTimer = burnDuration;

        float finalDamage = AppliedMagicResistance(ctrl, fireDamage.GetValue());
        ctrl.TakeDamage(Mathf.RoundToInt(finalDamage));

        // Apply FX
        ctrl.entityFX.StartCoroutine(nameof(entityFX.BurningFxFor), ctrl.burnTimer);
    }

    public virtual void DoIceDamage(StatsController ctrl)
    {
        // Update Freezing Data
        ctrl.isChilled = true;
        ctrl.chillTimer = chillDuration;
        float slowPercentage = .2f;
        ctrl.entity.SlowBy(slowPercentage, ctrl.chillTimer);

        float finalDamage = AppliedMagicResistance(ctrl, iceDamage.GetValue());
        ctrl.TakeDamage(Mathf.RoundToInt(finalDamage));

        // Apply FX
        ctrl.entityFX.StartCoroutine(nameof(entityFX.FreezingFxFor), ctrl.chillTimer);
    }

    public virtual void DoLightningDamage(StatsController ctrl)
    {
        // Update shocking data
        ctrl.isShocked = true;
        ctrl.shockTimer = shockDuration;
        float slowPercentage = .1f;
        ctrl.entity.SlowBy(slowPercentage, ctrl.shockTimer);

        float finalDamage = AppliedMagicResistance(ctrl, lightningDamage.GetValue());
        ctrl.TakeDamage(Mathf.RoundToInt(finalDamage));

        // Apply FX
        ctrl.entityFX.StartCoroutine(nameof(entityFX.ShockingFxFor), ctrl.shockTimer);
    }

    public virtual void DoRandomMagicDamage(StatsController ctrl)
    {
        // Random ability
        List<string> abilities = new();
        if (fireDamage.GetValue() > 0) abilities.Add("fire");
        if (iceDamage.GetValue() > 0) abilities.Add("ice");
        if (lightningDamage.GetValue() > 0) abilities.Add("lightning");

        string rdAffect = abilities[Random.Range(0, abilities.Count)];
        if (rdAffect == "fire")
            DoFireDamage(ctrl);
        else if (rdAffect == "ice")
            DoIceDamage(ctrl);
        else if (rdAffect == "lightning")
            DoLightningDamage(ctrl);
    }

    protected virtual float AppliedMagicResistance(StatsController ctrl, float magicDamage)
    {
        float finalDamage = magicDamage - ctrl.intelligent.GetValue() * 3;
        if (finalDamage < 0)
            return 0;
        return finalDamage;
    }

    // Physical Attacks
    public virtual void DoDamage(StatsController ctrl)
    {
        if (IsMissedAttack(ctrl)) { return; }

        float totalDamage = damage.GetValue() + strength.GetValue();

        if (IsCritical())
            totalDamage *= (critPower.GetValue() + strength.GetValue()) * .01f;

        float finalDamage = AppliedArmor(ctrl, totalDamage);
        ctrl.TakeDamage(Mathf.RoundToInt(finalDamage));
    }

    protected virtual bool IsMissedAttack(StatsController ctrl)
    {
        float evasionPoints = ctrl.evasion.GetValue() + ctrl.agility.GetValue();
        // decrease 20% accuracy
        if (isShocked)
        {
            Debug.Log(">> " + gameObject.name + " -20% accuracy");
            evasionPoints *= 1.2f;
        }
        return (Random.Range(0, 100) < evasionPoints);
    }

    public virtual float AppliedArmor(StatsController ctrl, float currDamage)
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

    protected virtual bool IsCritical()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        return (Random.Range(0, 100) < totalCriticalChance);
    }

    public virtual void TakeDamage(int damage, bool triggerAffect = true)
    {
        if (isDeath) return;

        if (triggerAffect)
            entity.DamageEffect();

        DecreaseHealth(damage);
        Debug.Log(gameObject.name + " HP: " + currentHp);

        if (currentHp <= 0)
            Die();
    }

    protected virtual void DecreaseHealth(int damage)
    {
        currentHp -= damage;
        onHealthChanged?.Invoke();
    }

    public virtual void IncreaseHealth(int value)
    {
        currentHp += value;
        int maxHealth = GetTotalMaxHealth();
        if (currentHp > maxHealth)
            currentHp = maxHealth;
        onHealthChanged?.Invoke();
    }

    protected virtual void Die()
    {
        isDeath = true;
        Debug.Log(gameObject.name + " died.");
    }

    public bool IsDeath()
    {
        return isDeath;
    }

}

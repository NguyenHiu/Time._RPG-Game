using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public Stat strength;
    public Stat damage;
    public Stat hp;

    [SerializeField] private float currentHp;

    protected virtual void Start()
    {
        currentHp = hp.GetValue();
    }

    public virtual void DoDamage(StatsController ctrl)
    {
        float totalDamage = damage.GetValue() + strength.GetValue();
        ctrl.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(float damage)
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

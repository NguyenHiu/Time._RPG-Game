using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public Stat damage;
    public Stat hp;

    [SerializeField] private float currentHp;

    private void Start()
    {
        currentHp = hp.GetValue();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHp);

        if (currentHp < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died.");
    }

}

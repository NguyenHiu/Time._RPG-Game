using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void TriggerAnim() => enemy.TriggerCurrentAnim();

    public void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);

        foreach (var obj in colliders)
        {
            Player e = obj.GetComponent<Player>();
            if (e != null)
            {
                e.Damage();
            }
        }
    }
}

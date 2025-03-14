using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Data/Item Effects/Freeze Enemy")]
public class FreezeEnemy : ItemEffect
{
    [Range(0f, 100f)]
    [SerializeField] private float triggeredHealth;
    [SerializeField] private float slowDownDuration;
    [Range(0f, .9f)]
    [SerializeField] private float slowDownPercent;
    [SerializeField] private float radius;

    public override void ExecuteEffect(Transform _transform)
    {
        if (PlayerManager.instance.player.statCtrl.GetCurrentHealthPercent() > triggeredHealth)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.SlowBy(slowDownPercent, slowDownDuration);
            }
        }
    }
}

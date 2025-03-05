using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void TriggerAnim() => enemy.TriggerCurrentAnim();

    private void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);

        foreach (var obj in colliders)
        {
            if (obj.TryGetComponent<StatsController>(out var sc))
            {
                enemy.statCtrl.DoRandomMagicAttack(sc);
            }
        }
    }

    private void OpenCounterArea()
    {
        enemy.OpenCounterArea();
    }
    private void CloseCounterArea()
    {
        enemy.CloseCounterArea();
    }
}

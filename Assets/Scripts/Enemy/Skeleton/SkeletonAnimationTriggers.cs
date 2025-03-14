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
            if (obj.TryGetComponent<Player>(out var p))
            {
                enemy.statCtrl.DoDamage(p.statCtrl);
                if (Random.Range(0, 100) < 50)
                    enemy.statCtrl.DoRandomMagicDamage(p.statCtrl);
            }
        }
    }

    public void OpenCounterArea()
    {
        enemy.OpenCounterArea();
    }

    public void CloseCounterArea()
    {
        enemy.CloseCounterArea();
    }
}

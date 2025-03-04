using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void TriggerAnim()
    {
        player.TriggerCurrentAnim();
    }

    private void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach (var obj in colliders)
        {
            if (obj.TryGetComponent<StatsController>(out var so))
            {
                so.TakeDamage(player.statCtrl.damage.GetValue());
            }
            // Trigger the Damage() method for testing only
            if (obj.TryGetComponent<Enemy>(out var e))
            {
                e.Damage();
            }
        }
    }

    private void TriggerThrowSword()
    {
        SkillManager.instance.throwSwordSkill.CreateSword(PlayerManager.instance.player.transform.position);
    }
}

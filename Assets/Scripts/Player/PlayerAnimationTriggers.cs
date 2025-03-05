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
            if (obj.TryGetComponent<StatsController>(out var sc))
            {
                //player.statCtrl.DoDamage(sc);
                player.statCtrl.DoFireAttack(sc);
            }
        }
    }

    private void TriggerThrowSword()
    {
        SkillManager.instance.throwSwordSkill.CreateSword(PlayerManager.instance.player.transform.position);
    }
}

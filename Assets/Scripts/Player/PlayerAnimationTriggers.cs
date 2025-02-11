using System.Collections;
using System.Collections.Generic;
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
            Enemy e = obj.GetComponent<Enemy>();
            if (e != null)
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

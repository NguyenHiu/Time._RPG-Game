using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var obj in colliders)
        {
            Enemy e = obj.GetComponent<Enemy>();
            if (e != null && e.CanBeStunned())
            {
                e.BeCounter();
                stateTimer = 10;
                player.anim.SetBool("SuccessfulCounterAttack", true);
            }
        }

        if (stateTimer < 0 || triggeredAnim)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}

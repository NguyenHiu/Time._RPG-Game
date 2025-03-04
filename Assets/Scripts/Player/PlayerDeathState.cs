using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player.capsuleCD.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void TriggerAnim()
    {
        base.TriggerAnim();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
    }
}

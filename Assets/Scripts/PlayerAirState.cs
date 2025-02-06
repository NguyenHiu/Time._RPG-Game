using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);

            if (player.IsWall())
            {
                stateMachine.ChangeState(player.wallSlice);
            }
        }
    }
}

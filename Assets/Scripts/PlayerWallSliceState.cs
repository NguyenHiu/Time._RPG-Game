using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWallSliceState : PlayerState
{
    public PlayerWallSliceState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        
        if (yInput < 0)
        {
            player.SetVelocity(rb.velocity.x, rb.velocity.y);
        }
        else
        {
            player.SetVelocity(rb.velocity.x, rb.velocity.y * 0.7f);
        }

        if (player.IsGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        } else if (!player.IsWall())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (xInput != 0 && xInput != player.facingDir)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJump);
        }

    }
}

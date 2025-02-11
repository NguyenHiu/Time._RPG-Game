using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && TryCreateSword())
        {
            stateMachine.ChangeState(player.aimSwordState);
        }
    }

    // TryCreateSword returns true if player can create new sword
    //                returns false and retrieves the existing sword if it exists
    private bool TryCreateSword()
    {
        // If the sword has already existed
        if (player.sword == null)
            return true;

        // Retrieve the existing sword
        player.sword.GetComponent<ThrowSwordController>().ReturnToPlayer();
        return false;
    }
}

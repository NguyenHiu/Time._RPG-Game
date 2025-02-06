using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    private float lastComboTime;
    private int comboCounter;
    private float comboStreakTime = 2;
    private int maxCombo = 3;

    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;
        
        if (comboCounter >= maxCombo || Time.time > lastComboTime + comboStreakTime)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("comboCounter", comboCounter);
        float attackDir = player.facingDir;
        if (xInput != 0) attackDir = xInput;
        player.SetVelocity(attackDir * player.attackMovements[comboCounter].x, player.attackMovements[comboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastComboTime = Time.time;
        player.StartCoroutine("BusyFor", .15f);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) {
            player.SetVelocity(0, 0);
        }

        if (triggeredAnim)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}

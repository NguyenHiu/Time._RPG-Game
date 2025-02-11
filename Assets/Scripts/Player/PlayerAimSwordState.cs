using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.throwSwordSkill.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.throwSwordSkill.DotsActive(false);
        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.transform.position;
        if (mousePos.x * player.facingDir < playerPos.x * player.facingDir)
            player.Flip();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);
    }
}

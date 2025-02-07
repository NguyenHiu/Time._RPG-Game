using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private SkeletonEnemy enemy;
    private GameObject player;
    private float moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, SkeletonEnemy _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player");
        Debug.Log("I'm in battle mode!");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D rc = enemy.IsPlayerDetected();
        if (rc)
        {
            stateTimer = enemy.battleTime;
            if (rc.distance < enemy.attackRange && Time.time > enemy.attackCooldown + enemy.lastTimeAttack)
            {
                stateMachine.ChangeState(enemy.attackState);
                return;
            } 
         } else
        {
            if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > enemy.battleRange)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        if (player.transform.position.x > enemy.rb.position.x)
        {
            moveDir = 1;
        } else
        {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.battleSpeed * moveDir, enemy.rb.velocity.y);
    }
}

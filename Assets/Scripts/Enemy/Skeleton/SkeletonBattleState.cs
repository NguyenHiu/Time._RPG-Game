using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private SkeletonEnemy enemy;
    private Transform playerTransform;
    private float moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, SkeletonEnemy _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        playerTransform = PlayerManager.instance.player.transform;
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
            if (
                rc.distance < enemy.attackRange &&
                Time.time > enemy.attackCooldown + enemy.lastTimeAttack &&
                // Ensure that the enemy is facing to the player
                rc.rigidbody.position.x * enemy.facingDir > enemy.transform.position.x * enemy.facingDir
            )
            {
                stateMachine.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, playerTransform.position) > enemy.battleRange)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        if (playerTransform.position.x > enemy.rb.position.x)
        {
            moveDir = 1;
        }
        else
        {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.battleSpeed * moveDir, enemy.rb.velocity.y);
    }
}

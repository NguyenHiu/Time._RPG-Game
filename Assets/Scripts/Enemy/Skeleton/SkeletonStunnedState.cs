using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private SkeletonEnemy enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, SkeletonEnemy _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.stunnedDuration;
        enemy.rb.velocity = new Vector2(
            enemy.rb.velocity.x - enemy.facingDir * enemy.stunnedDir.x,
            enemy.rb.velocity.y + enemy.stunnedDir.y
        );
        enemy.fx.InvokeRepeating("RedBlink", 0, .1f);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

}

using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private SkeletonEnemy enemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, SkeletonEnemy _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggeredAnim)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}

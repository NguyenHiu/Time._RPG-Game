public class SkeletonGroundState : EnemyState
{
    protected SkeletonEnemy enemy;

    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, SkeletonEnemy _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        enemy = _enemy;
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

        bool detect = enemy.IsPlayerDetected();
        if (detect)
            stateMachine.ChangeState(enemy.battleState);
    }
}

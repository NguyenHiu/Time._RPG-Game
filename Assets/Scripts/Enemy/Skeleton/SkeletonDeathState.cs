using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    private SkeletonEnemy enemy;

    public SkeletonDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, SkeletonEnemy enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.capsuleCD.enabled = false;
        enemy.anim.SetBool(enemy.lastState, true);
        enemy.SetFreeze(true);
        stateTimer = .2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
            enemy.rb.velocity = new Vector2(0, 10);
    }
}

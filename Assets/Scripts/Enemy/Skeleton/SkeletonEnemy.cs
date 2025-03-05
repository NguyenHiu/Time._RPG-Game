using UnityEngine;

public class SkeletonEnemy : Enemy
{

    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deathState = new SkeletonDeathState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + facingDir * attackRange, transform.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            new Vector3(transform.position.x - battleRange, transform.position.y - 0.2f),
            new Vector3(transform.position.x + battleRange, transform.position.y - 0.2f)
        );
    }

    public override void BeCounter()
    {
        base.BeCounter();
        stateMachine.ChangeState(stunnedState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }
}

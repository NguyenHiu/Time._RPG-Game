using UnityEngine;

public class EnemyState
{
    public EnemyStateMachine stateMachine { get; private set; }
    public Enemy enemyBase { get; private set; }

    protected bool triggeredAnim;
    protected float stateTimer;
    private string animName;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.animName = _animName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggeredAnim = false;
        enemyBase.anim.SetBool(animName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animName, false);
    }

    public void TriggerAnim() => triggeredAnim = true;
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyState
{
    public EnemyStateMachine stateMachine { get; private set; }
    public Enemy enemy { get; private set; }

    protected bool triggeredAnim;
    protected float stateTimer;
    private string animName;

    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemy, string _animName)
    {
        this.stateMachine = _stateMachine;
        this.enemy = _enemy;
        this.animName = _animName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggeredAnim = false;
        enemy.anim.SetBool(animName, true);
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animName, false);
    }

    public void TriggerAnim() => triggeredAnim = true;
}

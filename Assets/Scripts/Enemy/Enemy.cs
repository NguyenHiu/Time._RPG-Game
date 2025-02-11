using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine {  get; private set; }

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;

    [Header("Battle info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    public float detectPlayerForwardDistance;
    public float detectPlayerBehindDistance;
    public float battleTime;
    public float battleRange;
    public float battleSpeed;

    [Header("Attack info")]
    public float attackRange;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttack;
    [SerializeField] protected GameObject counterArea;
    protected bool canBeStunned;


    [Header("Stunned info")]
    public float stunnedDuration;
    public Vector2 stunnedDir;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    #region Collision Checks
    public virtual RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D forward = Physics2D.Raycast(wallCheck.position, new Vector2(facingDir, 0), detectPlayerForwardDistance, whatIsPlayer);
        if (forward)
        {
            return forward;
        }

        RaycastHit2D behind = Physics2D.Raycast(wallCheck.position, new Vector2(-facingDir, 0), detectPlayerBehindDistance, whatIsPlayer);
        return behind;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + facingDir * detectPlayerForwardDistance, wallCheck.position.y));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - facingDir * detectPlayerBehindDistance, wallCheck.position.y));
    }
    #endregion

    public void TriggerCurrentAnim() => stateMachine.currentState.TriggerAnim();

    public virtual void OpenCounterArea()
    {
        canBeStunned = true;
        counterArea.SetActive(true);
    }

    public virtual void CloseCounterArea()
    {
        canBeStunned = false;
        counterArea.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        return canBeStunned;
    }

    public virtual void BeCounter()
    {
        canBeStunned = false;
        CloseCounterArea();
    }
}

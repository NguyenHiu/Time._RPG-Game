using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine {  get; private set; }

    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;

    [Header("Attack info")]
    public float playerDetectDistance;
    public float attackRange;

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
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, new Vector2(facingDir, 0), playerDetectDistance, whatIsPlayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + facingDir * playerDetectDistance, wallCheck.position.y));
    }
    #endregion

    public void TriggerCurrentAnim() => stateMachine.currentState.TriggerAnim();
}

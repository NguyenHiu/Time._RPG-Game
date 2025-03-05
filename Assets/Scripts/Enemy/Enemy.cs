using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine { get; private set; }

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    private float defaultMoveSpeed;

    [Header("Battle info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    public float detectPlayerForwardDistance;
    public float detectPlayerBehindDistance;
    public float battleTime;
    public float battleRange;
    public float battleSpeed;
    private float defaultBattleSpeed;

    [Header("Attack info")]
    public float attackRange;
    public float attackCooldown;
    public float lastTimeAttack;
    [SerializeField] protected GameObject counterArea;
    protected bool canBeStunned;


    [Header("Stunned info")]
    public float stunnedDuration;
    public Vector2 stunnedDir;

    public string lastState;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
        defaultMoveSpeed = moveSpeed;
        defaultBattleSpeed = battleSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public void SetFreeze(bool isFreezed)
    {
        if (isFreezed)
        {
            moveSpeed = 0;
            battleSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            battleSpeed = defaultBattleSpeed;
            anim.speed = 1;
        }
    }

    public IEnumerator SetFreezeFor(float _seconds)
    {
        SetFreeze(true);
        yield return new WaitForSeconds(_seconds);
        SetFreeze(false);
    }


    #region Collision Checks
    public virtual RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D forward = Physics2D.Raycast(wallCheck.position, facingDir * Vector2.right, detectPlayerForwardDistance, whatIsPlayer);
        if (forward)
        {
            return forward;
        }

        RaycastHit2D behind = Physics2D.Raycast(wallCheck.position, -facingDir * Vector2.right, detectPlayerBehindDistance, whatIsPlayer);
        return behind;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + detectPlayerForwardDistance, wallCheck.position.y));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - detectPlayerBehindDistance, wallCheck.position.y));
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

    public override void SetVelocity(float _xVelocity, float _yVelocity)
    {
        // Reduce 20% speed while freezing
        if (statCtrl.isChilled)
        {
            Debug.Log(">> " + gameObject.name + " -20% speed");
            _xVelocity *= .8f;
        }
        else if (statCtrl.isShocked)
        {
            Debug.Log(">> " + gameObject.name + " -10% speed");
            _xVelocity *= .9f;
        }
        base.SetVelocity(_xVelocity, _yVelocity);
    }
}

using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack info")]
    public Vector2[] attackMovements;
    public bool isBusy { get; private set; }
    public float swordReturnedImpact;

    [Header("Counter attack info")]
    public float counterAttackDuration;

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    public float dashDir;
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;

    public GameObject sword;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSliceState wallSliceState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleUltimateState blackHoleUltimateState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        fallState = new PlayerFallState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSliceState = new PlayerWallSliceState(this, stateMachine, "WallSlice");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleUltimateState = new PlayerBlackHoleUltimateState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckDashState();

        if (Input.GetKeyDown(KeyCode.F))
        {
            SkillManager.instance.crystalSkill.CanUseSkill();
        }
    }

    public void AssignNewSword(GameObject _sword)
    {
        sword = _sword;
    }

    public void ClearTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public void CheckDashState()
    {

        dashDir = Input.GetAxisRaw("Horizontal");
        if (dashDir == 0)
            dashDir = facingDir;

        if (IsWallDetected()) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
        {
            stateMachine.ChangeState(dashState);
        }
    }

    public void TriggerCurrentAnim() => stateMachine.currentState.TriggerAnim();

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

    public override void SlowBy(float _slowPercentage, float _duration)
    {
        base.SlowBy(_slowPercentage, _duration);

        moveSpeed *= (1 - _slowPercentage);
        jumpForce *= (1 - _slowPercentage);
        dashSpeed *= (1 - _slowPercentage);

        StartCoroutine(CancelSlow(1/(1-_slowPercentage), _duration));
    }

    public override IEnumerator CancelSlow(float _restorePercentage, float _duration)
    {
        yield return base.CancelSlow(_restorePercentage, _duration);
        moveSpeed *= _restorePercentage;
        jumpForce *= _restorePercentage;
        dashSpeed *= _restorePercentage;
    }
}

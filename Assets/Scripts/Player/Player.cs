using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack info")]
    public Vector2[] attackMovements;
    public bool isBusy {  get; private set; }

    [Header("Counter attack info")]
    public float counterAttackDuration;

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    public float dashDir;
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;

    #region States
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState idleState {  get; private set; }
    public PlayerMoveState moveState {  get; private set; }
    public PlayerJumpState jumpState {  get; private set; }
    public PlayerFallState airState {  get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSliceState wallSlice { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerFallState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlice = new PlayerWallSliceState(this, stateMachine, "WallSlice");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
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
}

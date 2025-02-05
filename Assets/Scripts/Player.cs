using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack info")]
    public Vector2[] attackMovements;
    public bool isBusy {  get; private set; }

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    public float dashDir;
    private float dashTimer;
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    [Header("Collision info")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    #endregion

    #region States
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState idleState {  get; private set; }
    public PlayerMoveState moveState {  get; private set; }
    public PlayerJumpState jumpState {  get; private set; }
    public PlayerAirState airState {  get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSliceState wallSlice { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlice = new PlayerWallSliceState(this, stateMachine, "WallSlice");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        stateMachine.currentState.Update();
        CheckDashState();
    }

    public void CheckDashState()
    {

        dashDir = Input.GetAxisRaw("Horizontal");
        if (dashDir == 0)
        {
            dashDir = facingDir;
        }
        dashTimer -= Time.deltaTime;
        
        if (IsWall()) { return; }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer < 0)
        {
            dashTimer = dashCooldown;
            stateMachine.ChangeState(dashState);
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGrounded() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWall() => Physics2D.Raycast(wallCheck.position, new Vector2(facingDir, 0), wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x+wallCheckDistance*facingDir, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        } else if (_x < 0 && facingRight)
        {
            Flip();
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

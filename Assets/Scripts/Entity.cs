using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CapsuleCollider2D capsuleCD { get; private set; }
    #endregion

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnockback;

    [Header("Stats contronller")]
    [SerializeField] public StatsController statCtrl;

    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;
    public System.Action onFlipped;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        statCtrl = GetComponent<StatsController>();
        capsuleCD = GetComponent<CapsuleCollider2D>();
    }

    public virtual void DamageEffect()
    {
        fx.StartCoroutine(nameof(fx.Flash));
        StartCoroutine(nameof(Knockback));
    }

    public virtual void Die() { }

    protected virtual IEnumerator Knockback()
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackDir.x * -facingDir, knockbackDir.y);
        yield return new WaitForSeconds(.07f);
        isKnockback = false;
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    public virtual void SetZeroVelocity()
    {
        if (isKnockback) return;

        rb.velocity = new Vector2(0, 0);
    }

    public virtual void SetTransparent(bool isTransparent)
    {
        if (isTransparent) spriteRenderer.color = Color.clear;
        else spriteRenderer.color = Color.white;
    }

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnockback) return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, new Vector2(facingDir, 0), wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }

    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        onFlipped?.Invoke();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public virtual void SlowBy(float _slowPercentage, float _duration)
    {
        anim.speed *= (1 - _slowPercentage);
    }

    public virtual IEnumerator CancelSlow(float _restorePercentage, float _duration)
    {
        yield return new WaitForSeconds(_duration);
        anim.speed = 1;
    }
}

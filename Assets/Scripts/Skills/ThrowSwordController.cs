using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordController : MonoBehaviour
{
    /// <summary>
    /// Flow of the throw sword skill:
    /// 1. Player presses mouse1 while in the GroundedState, changing the state to AimSwordState.
    /// 2. The SkillManager runs concurrently, and the ThrowSwordSkill script shows the aim curve,
    ///    allowing the player to choose the attack direction by holding mouse1.
    /// 3. When the player releases mouse1, the ThrowSwordSkill calculates the `finalDir` to throw the sword.
    /// 4. After exiting the AimSwordState, the throw animation plays and triggers the `TriggerThrowSword` function,
    ///    which calls `CreateSword` and passes the normalized `finalDir` multiplied by the `launchDir`.
    /// </summary>

    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D cCollider;

    // Regular throw type can rotate
    [Header("General")]
    private bool canRotate = true;
    private float freezeTime;

    [Header("Return info")]
    private float returnSpeed;
    private bool isReturning = false;

    [Header("Bounce info")]
    [SerializeField] private float bounceRadius = 10;
    private float bounceSpeed;
    private List<Transform> bounceTargets = new();
    private bool isBouncing;
    private int bounceTimes;
    private int currentTarget = 0;

    [Header("Pierce info")]
    private int pierceTimes;
    private bool isPiercing;

    [Header("Spin info")]
    private float spinMaxDistance;
    private bool isSpinning;
    private float spinTimer;
    private float hitTimer;
    private float hitCooldown;
    private bool wasStopped;
    private Vector3 spinDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cCollider = GetComponent<CircleCollider2D>();
    }

    private void MyTimeHasCome()
    {
        Destroy(gameObject);
    }

    // The launchForce param has already calculated based on the pointer position 
    public void SetupSword(
            Vector2 _pos,
            Vector2 launchForce,
            float _gravityScale,
            float _returnSpeed,
            float _freezeTime,
            float _destroyTime
    )
    {
        transform.position = _pos;
        rb.gravityScale = _gravityScale;
        rb.velocity = launchForce;
        returnSpeed = _returnSpeed;
        freezeTime = _freezeTime;

        if (!isPiercing)
            anim.SetBool("Rotation", true);

        spinDirection = new Vector2(Mathf.Clamp(rb.velocity.x, -1, 1), 0);
        Invoke("MyTimeHasCome", _destroyTime);
    }

    // SetupBounce is used to setup bounce info 
    public void SetupBounce(bool _isBouncing, int _bounceTimes, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceTimes = _bounceTimes;
        bounceSpeed = _bounceSpeed;
    }

    // SetupPierce is used to setup pierce info
    public void SetupPierce(int _pierceTimes)
    {
        isPiercing = true;
        pierceTimes = _pierceTimes;
    }

    // SetupSpin is used to setup spin info
    public void SetupSpin(float _spinMaxDistance, float _spinTimer, float _hitCooldown)
    {
        isSpinning = true;
        spinMaxDistance = _spinMaxDistance;
        spinTimer = _spinTimer;
        hitCooldown = _hitCooldown;
    }

    public void Update()
    {
        // The transform's right is aligned with the sword tip, so changing the right according
        // to the velocity makes the sword movements smoother
        if (canRotate)
            transform.right = rb.velocity;

        TryReturn();
        TryBounce();
        TrySpin();
    }

    private void TrySpin()
    {
        if (isSpinning)
        {
            // Prevent to go out the max distance
            if (
                !wasStopped &&
                Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) >= spinMaxDistance
            )
            {
                // Spinning on the spot
                StopTheSword(RigidbodyConstraints2D.FreezePosition);
            }


            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, transform.position + spinDirection, 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (Collider2D hit in hitEnemies)
                    {
                        if (hit.TryGetComponent<Enemy>(out var e))
                            FreezeDamage(e);
                    }
                }
            }

        }
    }

    private void FreezeDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("SetFreezeFor", freezeTime);
    }

    private void TryReturn()
    {
        if (isReturning)
        {
            Vector2 playerPos = PlayerManager.instance.player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * returnSpeed);

            // If the sword is closed to the player --> destroy it
            if (Vector2.Distance(transform.position, playerPos) < 1)
                // ClearTheSword() also destroys the sword object
                PlayerManager.instance.player.ClearTheSword();
        }
    }

    private void TryBounce()
    {
        if (!isBouncing || bounceTargets.Count <= 0)
            return;

        // If the boundTargets contains only one enemy
        // Then this enemy may takes more than 1 times damage even there
        //          is only `one` bound (read the following code to understand more)
        Transform currTargetTransform = bounceTargets[currentTarget];
        Vector2 nextTarget = currTargetTransform.position;
        transform.position = Vector2.MoveTowards(transform.position, nextTarget, Time.deltaTime * bounceSpeed);
        if (Vector2.Distance(transform.position, nextTarget) < .1f)
        {
            if (currTargetTransform.TryGetComponent<Enemy>(out var e))
                FreezeDamage(e);

            currentTarget++;
            bounceTimes--;
            if (bounceTimes <= 0)
            {
                // Are u sure about setting isBound to false?
                isBouncing = false;
                ReturnToPlayer();
            }
            else if (currentTarget == bounceTargets.Count)
                currentTarget = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent the collider still from hitting objects on the way back to the player
        if (isReturning)
            return;

        bool isHitEnemy = collision.TryGetComponent<Enemy>(out Enemy _enemy);

        // Get bounce targets within the circle defined by bounceRadius
        if (isBouncing)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, bounceRadius);
            foreach (Collider2D target in targets)
            {
                if (target.TryGetComponent<Enemy>(out var e))
                    bounceTargets.Add(e.transform);
            }
        }

        // If the throw type is pierce, if the collision is not the ground, 
        // then check if the perice times are still valid for the sword to go through this enemy
        if (isPiercing && pierceTimes > 0 && isHitEnemy)
        {
            FreezeDamage(_enemy);
            pierceTimes--;
            return;
        }

        if (isSpinning && isHitEnemy)
        {
            wasStopped = true;
        }

        StuckInto(collision);
    }

    private void StopTheSword(RigidbodyConstraints2D freezeType)
    {
        // Kinematic body type prevents the object to be affected by external forces (e.g. gravity)
        rb.bodyType = RigidbodyType2D.Kinematic;
        // Freeze 
        rb.constraints = freezeType;
        // Disable the circle collider to prevent the sword to be triggered twice
        cCollider.enabled = false;
        wasStopped = true;
    }

    // StuckInto sets the object to be stuck in the the provided collision
    private void StuckInto(Collider2D collision)
    {
        // TODO: check if the else is the same as the if statement
        if (!isSpinning)
            StopTheSword(RigidbodyConstraints2D.FreezeAll);
        else StopTheSword(RigidbodyConstraints2D.FreezePosition);

        // Damage the overlapping enemy
        if (collision.TryGetComponent<Enemy>(out var e))
            FreezeDamage(e);

        // Even if bouncing, without bounce targets, then stick it into the ground
        if (isBouncing && bounceTargets.Count > 0)
            return;

        if (isSpinning) return;

        // Set the parent object to the collapse object to be moved along with this object
        transform.parent = collision.transform;
        // Stop rotate animation
        canRotate = false;
        anim.SetBool("Rotation", false);
    }

    // ReturnToPlayer brings the sword back to the player
    public void ReturnToPlayer()
    {
        // Freeze all axes of the object, then set new position to move the object towards the player
        // Actually, we can set the body type to Kinematic and set current velocity to 0. This will be the same xD
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }
}

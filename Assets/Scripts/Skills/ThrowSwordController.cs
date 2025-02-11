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

    private bool canRotate = true;

    [Header("Return info")]
    [SerializeField] private float returnSpeed = 12;
    private bool isReturning = false;

    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed = 20;
    [SerializeField] private float bounceRadius = 10;
    private List<Transform> boundTargets = new();
    private bool isBouncing;
    private int bounceTimes;
    private int currentTarget = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cCollider = GetComponent<CircleCollider2D>();
    }

    // The launchForce param has already calculated based on the pointer position 
    public void SetupSword(Vector2 _pos, Vector2 launchForce, float _gravityScale)
    {
        transform.position = _pos;
        rb.gravityScale = _gravityScale;
        rb.velocity = launchForce;

        anim.SetBool("Rotation", true);
    }

    // SetupBounce is used to setup bounce info 
    public void SetupBounce(bool _isBouncing, int _bounceTimes)
    {
        isBouncing = _isBouncing;
        bounceTimes = _bounceTimes;
    }

    public void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            Vector2 playerPos = PlayerManager.instance.player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * returnSpeed);

            // If the sword is closed to the player --> destroy it
            if (Vector2.Distance(transform.position, playerPos) < 1)
                // ClearTheSword() also destroys the sword object
                PlayerManager.instance.player.ClearTheSword();
        }

        TryBounce();
    }

    private void TryBounce()
    {
        if (!isBouncing || boundTargets.Count <= 0)
            return;

        // If the boundTargets contains only one enemy
        // Then this enemy may takes more than 1 times damage even there
        //          is only `one` bound (read the following code to understand more)
        Vector2 nextTarget = boundTargets[currentTarget].position;
        transform.position = Vector2.MoveTowards(transform.position, nextTarget, Time.deltaTime * bounceSpeed);
        if (Vector2.Distance(transform.position, nextTarget) < .1f)
        {
            currentTarget++;
            bounceTimes--;
            if (bounceTimes <= 0)
            {
                // Are u sure about setting isBound to false?
                isBouncing = false;
                ReturnToPlayer();
            }
            else if (currentTarget == boundTargets.Count)
                currentTarget = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent the collider still from hitting objects on the way back to the player
        if (isReturning)
            return;

        if (isBouncing)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, bounceRadius);
            foreach (Collider2D target in targets)
            {
                if (target.TryGetComponent<Enemy>(out var e))
                    boundTargets.Add(e.transform);
            }
        }

        // Kinematic body type prevents the object to be affected by external forces (e.g. gravity)
        rb.bodyType = RigidbodyType2D.Kinematic;
        // Freeze all rotation 
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // Disable the circle collider to prevent the sword to be triggered twice
        cCollider.enabled = false;

        if (isBouncing && boundTargets.Count > 0)
            return;

        // Set the parent object to the collapse object to be moved along with this object
        transform.parent = collision.transform;
        // Stop rotate animation
        anim.SetBool("Rotation", false);
        canRotate = false;
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

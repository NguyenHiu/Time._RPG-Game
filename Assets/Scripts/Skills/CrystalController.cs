using UnityEngine;

public class CrystalController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;

    // Crystal Config
    private bool canExplode;
    private float growSpeed;
    private bool _isGrowing;
    private bool canMove;
    private float moveSpeed;
    private float duration;
    private Transform closestEnemy;
    private float damage;
    private bool canShock;

    private bool isExploding = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }

    // Called by CrystalSkill to setup a new crystal
    public void SetupCrystal(Vector2 _pos, bool _canExplode, float _growSpeed, bool _canMove, float _moveSpeed, float _duration, Transform _closestEnemy, float _damage, bool _canShock)
    {
        transform.position = _pos;
        canExplode = _canExplode;
        growSpeed = _growSpeed;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        duration = _duration;
        closestEnemy = _closestEnemy;
        _isGrowing = false;
        damage = _damage;
        canShock = _canShock;
    }

    public void TrySwapCrystal(Vector2 playerPos)
    {
        // Can't swap in the explosion
        if (isExploding) 
            return;

        Vector2 myPos = transform.position;
        transform.position = playerPos;
        PlayerManager.instance.player.transform.position = myPos;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        // Explode the crystal 
        if (duration < 0 && !_isGrowing && canExplode)
        {
            // Growing the crystal size make the better visualization
            _isGrowing = true;
            // Make sure the crystal can't move while exploding
            canMove = false;
            anim.SetTrigger("Explode");
        }
        // Increase the crystal scale
        if (_isGrowing && !canMove)
        {
            float growUnit = growSpeed * Time.deltaTime;
            transform.localScale = transform.localScale + new Vector3(growUnit, growUnit);
        }
        // Move the crystal toward the closest enemy
        if (canMove && closestEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
            // If the crystal is close enough --> explode it
            if (Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                canExplode = true;
                duration = -.1f;
            }
        }
    }

    // Called by cyrstalExplode animation to damage enemies inside the collider
    private void TriggerExplode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var obj in colliders)
        {
            if (obj.TryGetComponent<Enemy>(out var e))
            {
                isExploding = true;

                // Take damage from explosive
                e.statCtrl.TakeDamage(Mathf.RoundToInt(damage));

                // Take magical damage
                if (canShock)
                    PlayerManager.instance.player.statCtrl.DoLightingDamage(e.statCtrl);
            }
        }
    }

    // Called by crystalExplode animation to delete the crystal after explosion
    private void DeleteCrystal()
    {
        Destroy(gameObject);
    }
}

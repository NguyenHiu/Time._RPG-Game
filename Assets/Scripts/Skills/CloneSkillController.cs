using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float losingSpeed;
    [SerializeField] private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = .8f;
    private bool canCreateAnotherClone;
    private float chanceToCreateAnotherClone;
    private int facingDir;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        facingDir = 1;
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * losingSpeed);
            if (sr.color.a < 0)
                Destroy(gameObject);
        }
        FacingClosestTarget();
    }

    public void SetupClone(Vector2 _pos, float _cloneDuration, bool _canAttack, bool _canCreateAnotherClone, float _chanceToCreateAnotherClone)
    {
        transform.position = _pos;
        cloneTimer = _cloneDuration;
        if (_canAttack)
            anim.SetInteger("AttackCounter", Random.Range(1, 4));
        canCreateAnotherClone = _canCreateAnotherClone;
        chanceToCreateAnotherClone = _chanceToCreateAnotherClone;
    }

    // TriggerAnim() and TriggerAttack() are used in Attack Animation
    public void TriggerAnim()
    {

    }

    public void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (var obj in colliders)
        {
            if (obj.TryGetComponent<Enemy>(out var e))
                e.Damage();
        }

        // chance to create another clone
        if (canCreateAnotherClone && Random.Range(0, 100) < chanceToCreateAnotherClone)
        {
            Vector2 pos = transform.position;
            pos.x += facingDir * 1.5f;
            SkillManager.instance.cloneSkill.CreateClone(pos);
        }
    }

    private void FacingClosestTarget()
    {
        Transform closestTarget = SkillManager.instance.cloneSkill.GetTheClosestEnemy(transform.position);

        if (closestTarget != null)
        {
            // Right is the default facing direction of the sprite
            // So if the target is on the left of the sprite, we rotate the facing direction!
            if (closestTarget.position.x * facingDir < transform.position.x * facingDir)
            {
                transform.Rotate(0, 180, 0);
                facingDir *= -1;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 25);
    }
}

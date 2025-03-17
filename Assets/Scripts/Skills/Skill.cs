using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected virtual void Start() { }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool TryUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual bool CanUseSkill() => cooldownTimer < 0;

    protected virtual void UseSkill() { }

    public virtual Transform GetTheClosestEnemy(Vector2 currPos)
    {
        Transform closestTarget = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currPos, 25);

        float closestDistance = Mathf.Infinity;
        foreach (var obj in colliders)
        {
            if (obj.TryGetComponent<Enemy>(out var e) && !e.statCtrl.IsDeath())
            {
                float distance = Vector2.Distance(e.transform.position, currPos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = e.transform;
                }
            }
        }

        return closestTarget;
    }
}

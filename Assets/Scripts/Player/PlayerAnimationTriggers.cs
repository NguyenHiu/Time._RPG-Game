using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void TriggerAnim()
    {
        player.TriggerCurrentAnim();
    }

    public void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach (var obj in colliders)
        {
            Enemy e = obj.GetComponent<Enemy>();
            if (e != null)
            {
                e.Damage();
            }
        }
    }
}

using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void TriggerAnim()
    {
        player.TriggerCurrentAnim();
    }

    private void TriggerAttack()
    {
        AudioManager.instance.PlaySFX(2, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach (var obj in colliders)
        {
            if (obj.TryGetComponent<Enemy>(out var e))
            {
                player.statCtrl.DoDamage(e.statCtrl);
                EquipmentItemData equippedWeapon = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
                if (equippedWeapon != null)
                    equippedWeapon.ExecuteEffects(e.transform);
            }
        }
    }

    private void TriggerThrowSword()
    {
        SkillManager.instance.throwSwordSkill.CreateSword(PlayerManager.instance.player.transform.position);
    }
}

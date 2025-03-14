using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject itemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player p) && !p.statCtrl.IsDeath())
            itemObject.PickUpItem();
    }
}

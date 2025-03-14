using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;

    private void OnValidate()
    {
        if (itemData == null)
            return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.name;
    }

    public void SetupItem(ItemData _itemData, Vector2 _dropVelocity)
    {
        itemData = _itemData;
        rb.velocity = _dropVelocity;
        OnValidate();
    }

    public void PickUpItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }

}

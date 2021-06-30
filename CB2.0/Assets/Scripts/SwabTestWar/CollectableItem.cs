using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Item itemMeta;

    public SpriteRenderer spriteRenderer;

    public void SetItem(Item _item)
    {
        itemMeta = _item;
        spriteRenderer.sprite = itemMeta.itemSprite;
    }
}

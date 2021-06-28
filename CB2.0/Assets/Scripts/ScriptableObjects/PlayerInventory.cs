using UnityEngine;

[
    CreateAssetMenu(
        fileName = "PlayerInventory",
        menuName = "ScriptableObjects/PlayerInventory",
        order = 3)
]
public class PlayerInventory : ScriptableObject
{
    public Item item;

    public Item GetCurrentItem()
    {
        return item;
    }

    public void SetItem(Item _item)
    {
        item = _item;
    }

    public void ClearItem() {
        item = null;
    }

    public Item useItem() {
        Item _item = item;
        item = null;
        return _item;
    }

    public bool hasItem() {
        return item != null;
    }
}

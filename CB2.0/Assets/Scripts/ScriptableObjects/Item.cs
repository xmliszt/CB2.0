using UnityEngine;

[
    CreateAssetMenu(
        fileName = "InventoryItem",
        menuName = "ScriptableObjects/InventoryItem",
        order = 4)
]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        swabStick = 1,
        testSample = 2,
        testResult = 3,
        trash = 4,

        shopItem = 5,

        cake = 6,
        pizza = 7
    }

    public ItemType itemType;

    public Sprite itemSprite;

    public Sprite thoughtBubbleSprite;

    public string itemName = "";
}

using UnityEngine;

[
    CreateAssetMenu(
        fileName = "STSPlayerInventory",
        menuName = "ScriptableObjects/STS/PlayerInventory",
        order = 5)
]

public class STSPlayerInventory : ScriptableObject
{
    public enum InventoryItemType
    {
        pizza = 1,
        cake = 2,
        swabstick = 3,
        nullItem = 4
    }

    public InventoryItemType inventoryItemType;

    public bool holdingItem = false;
}

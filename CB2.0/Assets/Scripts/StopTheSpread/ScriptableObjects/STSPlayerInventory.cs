using UnityEngine;

[System.Serializable]
public class STSItem
{
    public STSFood stsFood;

    public STSItem()
    {
        stsFood = null;
    }

    public STSFood GetCurrentItem()
    {
        return stsFood;
    }

    public void SetFood(STSFood _stsFood)
    {
        stsFood = _stsFood;
    }

    public void ClearFood()
    {
        stsFood = null;
    }

    public STSFood UseFood()
    {
        STSFood _stsFood = stsFood;
        stsFood = null;
        return _stsFood;
    }

    public bool hasFood()
    {
        return stsFood != null;
    }
}

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

    public STSItem stsItem = new STSItem();
}

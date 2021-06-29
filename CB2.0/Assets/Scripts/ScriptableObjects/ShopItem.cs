
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "ShopItem",
        menuName = "ScriptableObjects/ShopItem",
        order = 6)
]

public class ShopItem : ScriptableObject
{
    public Sprite itemSprite;
    public int cost; // number of coins it costs

    public bool unlimited = true; // if the item is unlimited, if false, stock will be considered

    public int stock = 0;
}

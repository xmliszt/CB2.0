using UnityEngine;

[
    CreateAssetMenu(
        fileName = "STSFood",
        menuName = "ScriptableObjects/STS/Food",
        order = 4)
]
public class STSFood : ScriptableObject
{
    public enum FoodType
    {
        pizza = 1,
        cake = 2
    }

    public FoodType foodType;

    public Sprite itemSprite;

    public Sprite thoughtBubbleSprite;
}
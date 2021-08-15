using UnityEngine;

public enum ControllerType
{
    xbox = 1,
    playStation = 2,

    generic = 3,

    keyboard = 4,

    nullType = 5,
}

public enum ControllerKeyType
{
    north = 1,
    south = 2,
    east = 3,
    west = 4,
    use = 5,
    pickup = 6,
    shop = 7,
    dash = 8,
}

[
    CreateAssetMenu(
        fileName = "ControllerKey",
        menuName = "ScriptableObjects/ControllerKey",
        order = 4)
]
public class ControllerKey : ScriptableObject {
    public ControllerType controllerType;
    public ControllerKeyType keyType;
    public Sprite keySprite;
 }

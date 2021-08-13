using UnityEngine;

public enum ControllerType
{
    xbox = 1,
    playStation = 2,

    generic = 3,
}

public enum ControllerKeyType
{
    north = 1,
    south = 2,
    east = 3,
    west = 4
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

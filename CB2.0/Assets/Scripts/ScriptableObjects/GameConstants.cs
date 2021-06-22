using UnityEngine;

[
    CreateAssetMenu(
        fileName = "GameConstants",
        menuName = "ScriptableObjects/GameConstants",
        order = 0)
]
public class GameConstants : ScriptableObject
{
    public float playerMoveSpeed = 3.0f;
}

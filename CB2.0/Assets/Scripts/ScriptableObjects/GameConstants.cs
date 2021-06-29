using UnityEngine;

[
    CreateAssetMenu(
        fileName = "GameConstants",
        menuName = "ScriptableObjects/GameConstants",
        order = 0)
]
public class GameConstants : ScriptableObject
{
    [Header("Game Generic Configuration")]

    public float gameBoundX = 10;
    public float gameBoundY = 5;

    [Header("Player Configuration")]
    public float playerMoveSpeed = 3.0f;

    public float playerDashSpeed = 10.0f;

    public float playerStunnedDuration = 3f; // seconds

    [Header("Particle System Configuration")]
    
    public float dashParticleOffset = 0.4f; 

    [Header("Floating Bubble Configuration")]

    public float floatingAmplitude = 0.5f;

    public float floatingFrequency = 1f;

    [Header("Interactables Configuration")]

    public float playerInZoneOpacity = 0.7f;

    [Header("Swab Stick Movement")]

    public float swabStickFlyingSpeed = 5;

    [Header("Test Station Configuration")]

    public float countdownDuration = 5; // seconds
}

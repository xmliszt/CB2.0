using UnityEngine;

[
    CreateAssetMenu(
        fileName = "GameConstants",
        menuName = "ScriptableObjects/GameConstants",
        order = 0)
]
public class GameConstants : ScriptableObject
{
    [Header("Player Configuration")]
    public float playerMoveSpeed = 3.0f;

    public float playerDashSpeed = 2.0f;

    public float playerDashDuration = 0.1f; // seconds

    [Header("Particle System Configuration")]
    
    public float dashParticleOffset = 0.4f; 

    [Header("Floating Bubble Configuration")]

    public float floatingAmplitude = 0.5f;

    public float floatingFrequency = 1f;

    [Header("Interactables Configuration")]

    public float playerInZoneOpacity = 0.7f;

    [Header("Swab Stick Movement")]

    public float swabStickFlyingSpeed = 5;
}

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

    public int minigameCountdownTime = 60 * 3; // in seconds

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

    public float playerInZoneOpacity = 0.7f; // the gameobject will be set to this opacity if the player is in the trigger zone


    // **************** Minigame: Swab Test War **********************

    [Header("Swab Stick Movement")]

    public float swabStickFlyingSpeed = 5;

    [Header("Test Station Configuration")]

    public float countdownDuration = 5; // seconds

    [Header("Item Spawner Configuration")]

    public float spawnFrequency = 5; // 1 spawn every seconds

    [Header("Coin Reward")]
    
    public int coinAwardedPerCompleteTest = 1;

    // **************** Minigame: Unlimited Group Size **********************

    [Header("Player Raycast")]

    public float castRadius = 0.3f;

    [Header("Entertainment Spawn")]
    
    public float entertainmentSpawnFreq = 5;

}

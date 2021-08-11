using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHGameManager : MonoBehaviour
{
    public GameEvent onShowRulePage;
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public Players players;

    public PlayerLocation[] playerLocations;
    public SpawnedPickupManager spawnManager;
    public ScoreManager scoreManager;
    public SnHNPCManager npcManager;

    // Start is called before the first frame update
    void Start()
    {
        foreach(KeyValuePair<int, PlayerInfo> player in players.GetPlayers())
        {
            int playerID = player.Key;
            PlayerInfo playerInfo = player.Value;
            PlayerStats playerStats = playerInfo.playerStats;
            Vector3 playerSpawnPosition = GetPlayerLocation(playerID);
            playerRelocateGameEvent.Fire(playerID, playerSpawnPosition);
        }

        onShowRulePage.Fire();
    }

    private Vector3 GetPlayerLocation(int playerID)
    {
        foreach(PlayerLocation playerLocation in playerLocations)
        {
            if (playerLocation.playerID == playerID)
            {
                return playerLocation.location.position;
            }
        }
        return Vector3.zero;
    }
}

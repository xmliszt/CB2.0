using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHGameManager : MonoBehaviour
{
    public GameEvent onShowRulePage;
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public GameEvent onSNHGameStart;
    public Players players;

    public PlayerLocation[] playerLocations;
    public SnHGameConstants gameConstants;
    public SnHBasketController P1BC, P2BC, P3BC, P4BC;
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

        // select random item for game constant
        int otheritem = Random.Range(0, 3); // game constant have to follow pickup index
        gameConstants.OtherIndex = otheritem + 2;

        int TPCollected = Random.Range(gameConstants.collectTotal / 2 - 2, gameConstants.collectTotal / 2 + 3);
        int remaining = gameConstants.collectTotal - TPCollected;
        gameConstants.CollectTP = TPCollected;
        gameConstants.CollectOther = remaining;

        // select random number of npcs
        int npcCount = Random.Range(1, 6);
        gameConstants.NPCs = npcCount;

        // spawn objects
        spawnManager.onStart();

        // spawn npcs
        npcManager.onStart();

        // initialise the scores
        scoreManager.onStart();

        // initialise the baskets
        P1BC.onStart();
        P2BC.onStart();
        P3BC.onStart();
        P4BC.onStart();

        // initialise the players - GameEvent
        onSNHGameStart.Fire();
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

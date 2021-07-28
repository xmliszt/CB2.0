using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class STSGameManager : MonoBehaviour
{
    public GameStats gameStats;
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public PlayerLocation[] playerLocations;
    public GameObject playerPrefab;
    public Players players;
    public GameEvent OnStartPlayStart;

    // Keep track of the spawned player gameobject
    private Dictionary<int, Transform> playerObjects;

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
        OnStartPlayStart.Fire();
        gameStats.SetCurrentScene(GameStats.Scene.stopTheSpread);    
    }

    public int GetNumberPlayers()
    {
        return playerObjects.Count;
    }

    public Vector3 GetPlayerLocation(int playerID)
    {
        foreach (PlayerLocation playerLocation in playerLocations)
        {
            if (playerLocation.playerID == playerID)
            {
                return playerLocation.location.position;
            }
        }
        return Vector3.zero;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupManager : MonoBehaviour
{
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public PlayerLocation[] playerLocations;
    public Players players;
    public GameEvent OnStartPlayStart;
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
    }

    public Vector3 GetPlayerLocation(int playerID)
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

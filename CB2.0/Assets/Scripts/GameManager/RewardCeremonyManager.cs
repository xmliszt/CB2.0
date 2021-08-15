using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RewardCeremonyManager : MonoBehaviour
{
    public GameEvent OnCeremonyStart;

    public GameEvent OnStart; // Start Player Control
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public PlayerLocation[] playerLocations;
    public GameObject playerPrefab;
    public Players players;

    private void Awake() {
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    void Start()
    {
        OnStart.Fire();
        OnCeremonyStart.Fire();
        foreach(KeyValuePair<int, PlayerInfo> player in players.GetPlayers())
        {
            int playerID = player.Key;
            PlayerInfo playerInfo = player.Value;
            PlayerStats playerStats = playerInfo.playerStats;
            Vector3 playerSpawnPosition = GetPlayerLocation(playerID);
            playerRelocateGameEvent.Fire(playerID, playerSpawnPosition);
        }
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

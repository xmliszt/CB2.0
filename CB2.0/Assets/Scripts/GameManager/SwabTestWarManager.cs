using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class PlayerLocation
{
    public int playerID;

    public Transform location;
}
public class SwabTestWarManager : MonoBehaviour
{
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public PlayerLocation[] playerLocations;
    public GameObject playerPrefab;
    public Players players;
    public GameEvent onShowRulePage;
    public TestStation[] testStations;

    private void Awake() {
        PhotonNetwork.IsMessageQueueRunning = true;
    }
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
        foreach (TestStation testStation in testStations)
        {
            testStation.isLoaded = false;
            testStation.isLocked = false;
            testStation.isComplete = false;
            testStation.resultOwner = 0;
            testStation.playersInZone = new List<int>();
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

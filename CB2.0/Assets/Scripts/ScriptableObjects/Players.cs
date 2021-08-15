using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfo
{
    public PlayerStats playerStats;

    public GameObject player;

    public PlayerInfo(PlayerStats _playerStats, GameObject _player)
    {
        playerStats = _playerStats;
        player = _player;
    }
}

[
    CreateAssetMenu(
        fileName = "Players",
        menuName = "ScriptableObjects/Players",
        order = 8)
]
public class Players : ScriptableObject
{
    private Dictionary<int, PlayerInfo>
        players = new Dictionary<int, PlayerInfo>();

    public bool PlayerExist(int playerID)
    {
        return players.ContainsKey(playerID);
    }

    public void AddPlayer(PlayerStats playerStats, GameObject player)
    {
        players[playerStats.playerID] = new PlayerInfo(playerStats, player);
    }

    public void UpdatePlayer(int playerID, GameObject player)
    {
        players[playerID].player = player;
    }

    public void UpdatePlayer(int playerID, PlayerStats playerStats)
    {
        if (
            !players.ContainsKey(playerID) ||
            playerStats != players[playerID].playerStats
        )
        {
            Debug.Log(string.Format("Player {0} update player stats: {1}", playerID, playerStats));
            players[playerID].playerStats = playerStats;
            players[playerID]
                .player
                .GetComponent<PlayerStatsManager>()
                .SetPlayerStats(playerStats);
        }
    }

    public void RemovePlayer(int playerID)
    {
        players.Remove (playerID);
    }

    public Dictionary<int, PlayerInfo> GetPlayers()
    {
        return players;
    }
}

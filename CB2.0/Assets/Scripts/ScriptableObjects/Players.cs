using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfo {
    public PlayerStats playerStats;
    public PlayerInput playerInput;

    public PlayerInfo(PlayerStats _playerStats, PlayerInput _playerInput)
    {
        playerStats = _playerStats;
        playerInput = _playerInput;
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
    private Dictionary<int, PlayerInfo> players = new Dictionary<int, PlayerInfo>();

    public void AddPlayer(PlayerStats playerStats, PlayerInput playerInput)
    {
        players[playerStats.playerID] = new PlayerInfo(playerStats, playerInput);
    }

    public void UpdatePlayer(int playerID, PlayerInput playerInput)
    {
        players[playerID].playerInput = playerInput;
    }

    public void UpdatePlayer(int playerID, PlayerStats playerStats)
    {
        players[playerID].playerStats = playerStats;
    }

    public void RemovePlayer(int playerID)
    {
        players.Remove(playerID);
    }

    public Dictionary<int, PlayerInfo> GetPlayers()
    {
        return players;
    }
}

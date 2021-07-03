using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
   
    private PlayerStats playerStats;
    
    public void SetPlayerStats(PlayerStats _playerStats)
    {
        playerStats = _playerStats;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}

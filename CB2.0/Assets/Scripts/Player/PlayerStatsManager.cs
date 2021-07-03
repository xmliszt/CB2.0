using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    public TMP_Text playerUIIndicatorText;
   
    private PlayerStats playerStats;
    
    public void SetPlayerStats(PlayerStats _playerStats)
    {
        playerStats = _playerStats;
        GetComponent<SpriteOutlined>().EnableOutline(playerStats);
        playerUIIndicatorText.text =
            string.Format("{0}P", playerStats.playerID);
        playerUIIndicatorText.color =
            playerStats.playerAccent;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}

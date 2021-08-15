using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public TMP_Text playerUIIndicatorText;

    public Animator playerAnimator;

    private PlayerStats playerStats;

    public void SetPlayerStats(PlayerStats _playerStats)
    {
        playerStats = _playerStats;
        GetComponent<SpriteOutlined>().EnableOutline(playerStats);
        playerAnimator.runtimeAnimatorController =
            playerStats.animatorController;
        playerUIIndicatorText.text = playerStats.playerName;
        playerUIIndicatorText.color = playerStats.playerAccent;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}

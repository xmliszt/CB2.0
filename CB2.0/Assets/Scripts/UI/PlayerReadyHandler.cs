using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyHandler : MonoBehaviour
{
    private bool isActivated = false;
    private PlayerStatsManager playerStatsManager;

    private PlayerAudioController playerAudioController;

    public Players players;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerAudioController = GetComponent<PlayerAudioController>();
    }

    public void OnPlayerReady()
    {
        if (isActivated && playerStatsManager)
        {
            if (!playerStatsManager.GetPlayerStats().ready)
            {
                Debug.Log(string.Format("Player {0} is getting ready!", playerStatsManager.GetPlayerStats().playerID));
                int readyIdx = GetCurrentReadyCount();
                playerAudioController.PlayReadySFX(readyIdx);
                playerStatsManager.GetPlayerStats().ready = true;
            }
        }
    }

    public void ActivatePlayerReadyHandler()
    {
        isActivated =  true;
    }

    public void DeactivatePlayerReadyHandler()
    {
        isActivated = false;
    }

    public void ResetPlayerReadyStatus()
    {
        if (playerStatsManager)
        {
            playerStatsManager.GetPlayerStats().ready = false;
        }
    }

    private int GetCurrentReadyCount()
    {
        int readyCount = 0;
        foreach(PlayerInfo _info in players.GetPlayers().Values)
        {
            if (_info.playerStats.ready)
            {
                readyCount ++;
            }
        }
        return readyCount;
    }
}

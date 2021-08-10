using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyHandler : MonoBehaviour
{
    private bool isActivated = false;
    private PlayerStatsManager playerStatsManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    public void OnPlayerReady()
    {
        if (isActivated && playerStatsManager)
        {
            if (!playerStatsManager.GetPlayerStats().ready)
            {
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralGameManager : MonoBehaviour
{
    private PlayerInput[] playerInputsList = new PlayerInput[4];

    public PlayerStats[] playerStatsList;

    private void Start()
    {
        foreach (PlayerStats playerStats in playerStatsList)
        {
            playerStats.selected = false;
            playerStats.playerID = 0;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        playerInputsList[playerInput.playerIndex] = playerInput;
        SwitchPlayerProfile (playerID);
        EnablePlayerController(playerInput.gameObject,
        ControllerType.GameLobbyController);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        ClearPlayerProfileAssignment (playerID);
    }

    public void OnPlayerSwitchProfile(int playerID)
    {
        SwitchPlayerProfile (playerID);
    }

    public void OnPlayerChangeZone(
        int playerID,
        string zoneType,
        GameObject zoneObject
    )
    {
        GameObject player = playerInputsList[playerID - 1].gameObject;

        player.GetComponent<PlayerZoneManager>().SetZone(zoneType, zoneObject);
    }

    private enum ControllerType
    {
        GameLobbyController = 1,
        SnatchAndHoardController = 2,
        SwabTestController = 3,
        UnlimitedGroupController = 4,
        StopTheSpreadController = 5
    }

    // Enable a controller script for the player according to minigame he/she is at
    private void EnablePlayerController(GameObject player, ControllerType _type)
    {
        GameLobbyPlayerController gameLobbyPlayerController =
            player.GetComponent<GameLobbyPlayerController>();
        SwabTestPlayerController swabTestPlayerController =
            player.GetComponent<SwabTestPlayerController>();

        // Disable all controllers first
        gameLobbyPlayerController.enabled = false;
        swabTestPlayerController.enabled = false;

        switch (_type)
        {
            case ControllerType.GameLobbyController:
                gameLobbyPlayerController.enabled = true;
                break;
            case ControllerType.SwabTestController:
                swabTestPlayerController.enabled = true;
                break;
        }
    }

    // Switch a character for a given player
    // Will set the current player's character to be unselected, remove playerID
    // Choose the next available character to assign to this player
    public void SwitchPlayerProfile(int playerID)
    {
        PlayerStats selectedPlayerStats = null;

        int lastIdx = -1;

        for (int idx = 0; idx < playerStatsList.Length; idx ++)
        {
            PlayerStats playerStats = playerStatsList[idx];
            if (playerStats.playerID == playerID)
            {
                playerStats.selected = false;
                playerStats.playerID = 0;
                lastIdx = idx;
            }
        }

        lastIdx ++;

        for (int i = 0; i < playerStatsList.Length - 2; i ++)
        {
            if (lastIdx == playerStatsList.Length) lastIdx = 0;
            PlayerStats playerStats = playerStatsList[lastIdx];
            if (!playerStats.selected)
            {
                Debug.Log("Selected " + playerStats.name);
                playerStats.playerID = playerID;
                playerStats.selected = true;
                selectedPlayerStats = playerStats;
                break;
            }
            lastIdx ++;
        }
        playerInputsList[playerID - 1]
            .GetComponent<PlayerStatsManager>()
            .SetPlayerStats(selectedPlayerStats);
    }

    // Call this when a player left the game
    public void ClearPlayerProfileAssignment(int playerID)
    {
        foreach (PlayerStats playerStats in playerStatsList)
        {
            if (playerStats.playerID == playerID)
            {
                // deselect current
                playerStats.selected = false;
                playerStats.playerID = 0;
            }
        }
    }

    // Call this at the start of each minigame to reset player's stats
    public void ResetPlayerStatsForMiniGame()
    {
        foreach (PlayerStats playerStats in playerStatsList)
        {
            playerStats.coins = 0;
            playerStats.inventory.ClearItem();
        }
    }

    // Call this when whole game restarted
    public void ResetPlayerStatsCompletely()
    {
        foreach (PlayerStats playerStats in playerStatsList)
        {
            playerStats.selected = false;
            playerStats.playerID = 0;
            playerStats.score = 0;
            playerStats.coins = 0;
            playerStats.inventory.ClearItem();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralGameManager : MonoBehaviour
{
    private PlayerInput[] playerInputsList = new PlayerInput[4];
    public PlayerStats[] playerStatsList;

    private void Start() {
        foreach (PlayerStats playerStats in playerStatsList)
        {
            playerStats.selected = false;
            playerStats.playerID = 0;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        PlayerStats assignedPlayerStats = SwitchPlayerProfile(playerID);
        playerInputsList[playerInput.playerIndex] = playerInput;
        EnablePlayerController(playerInput.gameObject, ControllerType.GameLobbyController, assignedPlayerStats);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        ClearPlayerProfileAssignment(playerID);
    }    

    public void OnPlayerSwitchProfile(int playerID)
    {
        GameObject player = playerInputsList[playerID - 1].gameObject;

        PlayerStats playerStats = SwitchPlayerProfile(playerID);

        // Set player stats for all controllers
        player.GetComponent<PlayerStatsManager>().SetPlayerStats(playerStats);
    }

    public void OnPlayerChangeZone(int playerID, string zoneType, GameObject zoneObject)
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
    private void EnablePlayerController(GameObject player, ControllerType _type, PlayerStats playerStats)
    {
        GameLobbyPlayerController gameLobbyPlayerController =
            player.GetComponent<GameLobbyPlayerController>();
        SwabTestPlayerController swabTestPlayerController =
            player.GetComponent<SwabTestPlayerController>();

        // Disable all controllers first
        gameLobbyPlayerController.enabled = false;
        swabTestPlayerController.enabled = false;

        // Set player stats for all controllers
        player.GetComponent<PlayerStatsManager>().SetPlayerStats(playerStats);

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
    public PlayerStats SwitchPlayerProfile(int playerID)
    {
        PlayerStats selectedPlayerStats = null;
        foreach (PlayerStats playerStats in playerStatsList)
        {
            if (playerStats.playerID == playerID)
            {
                // deselect current
                playerStats.selected = false;
                playerStats.playerID = 0;
            }
            else if (!playerStats.selected)
            {
                playerStats.playerID = playerID;
                playerStats.selected = true;
                selectedPlayerStats = playerStats;
                break;
            }
        }
        return selectedPlayerStats;
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

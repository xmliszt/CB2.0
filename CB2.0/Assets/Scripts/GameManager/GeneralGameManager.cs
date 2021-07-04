using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    // The list of player profiles to select from
    public PlayerStats[] playerProfiles;

    // The scriptable objects that will be remembered throughout the game
    public GameStats gameStats;

    public Players players;

    // Keep track of the spawned player gameobject
    private Dictionary<int, Transform> playerObjects;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        DontDestroyOnLoad (gameObject);
    }

    private void Start()
    {
        playerObjects = new Dictionary<int, Transform>();
        playerInputManager = GetComponent<PlayerInputManager>();
        gameStats.SetCurrentScene(GameStats.Scene.gameLobby);
        foreach (PlayerStats playerStats in playerProfiles)
        {
            playerStats.selected = false;
            playerStats.playerID = 0;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        playerObjects[playerID] = playerInput.gameObject.transform;
        PlayerStats newPlayerStats = SwitchPlayerProfile(playerID); // assign one profile to the joined player
        players.AddPlayer (newPlayerStats, playerInput);
        DontDestroyOnLoad(playerInput.gameObject);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        Debug.Log(string.Format("Player {0} left", playerID));
        ClearPlayerProfileAssignment (playerID);
        players.RemovePlayer (playerID);
    }

    public void OnPlayerSwitchProfile(int playerID)
    {
        PlayerStats newPlayerStats = SwitchPlayerProfile(playerID);
        players.UpdatePlayer (playerID, newPlayerStats);
    }

    // Switch a character for a given player
    // Will set the current player's character to be unselected, remove playerID
    // Choose the next available character to assign to this player
    public PlayerStats SwitchPlayerProfile(int playerID)
    {
        PlayerStats selectedPlayerStats = null;

        int lastIdx = -1;

        for (int idx = 0; idx < playerProfiles.Length; idx++)
        {
            PlayerStats playerStats = playerProfiles[idx];
            if (playerStats.playerID == playerID)
            {
                playerStats.selected = false;
                playerStats.playerID = 0;
                lastIdx = idx;
            }
        }

        lastIdx++;

        for (int i = 0; i < playerProfiles.Length - 1; i++)
        {
            if (lastIdx == playerProfiles.Length) lastIdx = 0;
            PlayerStats playerStats = playerProfiles[lastIdx];
            if (!playerStats.selected)
            {
                playerStats.playerID = playerID;
                playerStats.selected = true;
                selectedPlayerStats = playerStats;
                break;
            }
            lastIdx++;
        }
        playerObjects[playerID]
            .GetComponent<PlayerStatsManager>()
            .SetPlayerStats(selectedPlayerStats);
        return selectedPlayerStats;
    }

    // Call this when a player left the game
    public void ClearPlayerProfileAssignment(int playerID)
    {
        playerObjects[playerID] = null;
        foreach (PlayerStats playerStats in playerProfiles)
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
        foreach (PlayerStats playerStats in playerProfiles)
        {
            playerStats.coins = 0;
            playerStats.inventory.ClearItem();
        }
    }

    // Call this when whole game restarted
    public void ResetPlayerStatsCompletely()
    {
        foreach (PlayerStats playerStats in playerProfiles)
        {
            playerStats.selected = false;
            playerStats.playerID = 0;
            playerStats.score = 0;
            playerStats.coins = 0;
            playerStats.inventory.ClearItem();
        }
    }

    // Call this to start the minigame
    public void OnStartMiniGame()
    {
        if (playerInputManager.playerCount < 2)
        {
            Debug.Log("cannot start game. need at least 2 players");
        }
        else
        {
            SceneManager
                .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            gameStats.SetCurrentScene(GameStats.Scene.swabTestWar);
        }
    }

    public void OnPlayerRelocate(int playerID, Vector3 location)
    {
        Debug
            .Log(string
                .Format("Player {0} relocate to {1}", playerID, location));
        playerObjects[playerID].position = location;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupManager : MonoBehaviour
{
    public PlayerRelocateGameEvent playerRelocateGameEvent;
    public PlayerLocation[] playerLocations;
    public GameObject playerPrefab;
    public Players players;
    public GameEvent OnStartPlayStart;

    public GameStats gameStats;

        // The list of player profiles to select from
    public PlayerStats[] playerProfiles;

    // Keep track of the spawned player gameobject
    private Dictionary<int, Transform> playerObjects;

    private PlayerInputManager playerInputManager;
    void Start()
    {
        
        
        
        playerObjects = new Dictionary<int, Transform>();
        gameStats.SetCurrentScene(GameStats.Scene.unlimitedGroupSize);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        Debug.Log(string.Format("Player {0} joined", playerID));
        playerObjects[playerID] = playerInput.gameObject.transform;
        PlayerStats newPlayerStats = SwitchPlayerProfile(playerID); // assign one profile to the joined player
        players.AddPlayer(newPlayerStats, playerInput);

        // set player's starting position
        Vector3 playerSpawnPosition = GetPlayerLocation(playerID);
        OnPlayerRelocate(playerID, playerSpawnPosition);

        playerRelocateGameEvent.Fire(playerID, playerSpawnPosition);

        DontDestroyOnLoad(playerInput.gameObject);
    }

    public void OnPlayerSwitchProfile(int playerID)
    {
        PlayerStats newPlayerStats = SwitchPlayerProfile(playerID);
        players.UpdatePlayer(playerID, newPlayerStats);
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

    private Vector3 GetPlayerLocation(int playerID)
    {
        foreach(PlayerLocation playerLocation in playerLocations)
        {
            if (playerLocation.playerID == playerID)
            {
                return playerLocation.location.position;
            }
        }
        return Vector3.zero;
    }

    private void OnPlayerRelocate(int playerID, Vector3 location)
    {
        playerObjects[playerID].position = location;
    }
}

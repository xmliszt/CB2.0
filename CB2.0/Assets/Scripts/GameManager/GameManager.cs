using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // The list of player profiles to select from
    public PlayerStats[] playerProfiles;

    // The scriptable objects that will be remembered throughout the game
    public GameStats gameStats;

    public Players players;

    public List<GameStats.Scene> minigameSequence;

    public GameEvent onReturnGameLobby;

    public GameEvent onStartSTS;

    public GameEvent onGameLobbyInitialized;

    public GameEvent onPlayerJoinedEvent;

    private int currentMinigameSceneIdx;

    // Keep track of the spawned player gameobject
    private Dictionary<int, Transform> playerObjects;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        DontDestroyOnLoad (gameObject);
    }

    private void Start()
    {
        ResetPlayerStatsCompletely();
        playerObjects = new Dictionary<int, Transform>();
        playerInputManager = PlayerInputManager.instance;
        gameStats.SetCurrentScene(GameStats.Scene.gameLobby);
        foreach (PlayerStats playerStats in playerProfiles)
        {
            playerStats.selected = false;
            playerStats.playerID = 0;
        }
        onGameLobbyInitialized.Fire();
        foreach (PlayerInfo playerInfo in players.GetPlayers().Values)
        {
            OnPlayerJoined(playerInfo.playerInput);
        }
        Debug.Log("Game Manager Started");
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        onPlayerJoinedEvent.Fire();
        int playerID = playerInput.playerIndex + 1;
        Debug.Log(string.Format("Player {0} joined", playerID));
        if (playerObjects == null)
            playerObjects = new Dictionary<int, Transform>();
        playerObjects[playerID] = playerInput.gameObject.transform;
        PlayerStats newPlayerStats = SwitchPlayerProfile(playerID); // assign one profile to the joined player
        if (!players.PlayerExist(playerID))
            players.AddPlayer(newPlayerStats, playerInput);
        else
            players.UpdatePlayer (playerID, newPlayerStats);
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
            playerStats.score = 0;
            playerStats.inventory.ClearItem();
        }
    }

    // Call this when the whole game restarted
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
            gameStats.SetCurrentScene(minigameSequence[0]);
            currentMinigameSceneIdx = 0;
            GameStats.Scene firstScene = gameStats.GetCurrentScene();
            LoadMinigame (firstScene);
        }
    }

    public void LoadMinigame(GameStats.Scene sceneTag)
    {
        switch (sceneTag)
        {
            case GameStats.Scene.gameLobby:
                SceneManager.LoadScene("GameLobby");
                break;
            case GameStats.Scene.swabTestWar:
                SceneManager.LoadScene("SwabTestWar");
                break;
            case GameStats.Scene.stopTheSpread:
                SceneManager.LoadScene("StopTheSpread");
                onStartSTS.Fire();
                break;
        }
    }

    public void OnPlayNextMinigame()
    {
        int nextSceneIdx = currentMinigameSceneIdx + 1;
        currentMinigameSceneIdx = nextSceneIdx;

        if (nextSceneIdx == minigameSequence.Count)
        {
            currentMinigameSceneIdx = 0;
            gameStats.SetCurrentScene(GameStats.Scene.gameLobby);
            ResetPlayerStatsCompletely();
            onReturnGameLobby.Fire();
            LoadMinigame(GameStats.Scene.gameLobby);
            foreach(Transform player in playerObjects.Values)
            {
                Destroy(player);
            }
            Destroy (gameObject);
        }
        else
        {
            gameStats.SetCurrentScene(minigameSequence[nextSceneIdx]);
            ResetPlayerStatsForMiniGame();
            LoadMinigame(minigameSequence[nextSceneIdx]);
        }
    }

    public void OnPlayerRelocate(int playerID, Vector3 location)
    {
        playerObjects[playerID].position = location;
    }
}

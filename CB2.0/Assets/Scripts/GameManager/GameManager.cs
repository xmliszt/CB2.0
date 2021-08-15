using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPun
{
    public GameObject playerPrefab;

    // The list of player profiles to select from
    public PlayerStats[] playerProfiles;

    // The scriptable objects that will be remembered throughout the game
    public GameStats gameStats;

    public Players players;

    public PlayerLocation[] playerLocations;

    private List<GameStats.Scene> minigameSequence;

    private Dictionary<GameStats.Scene, bool> minigameSelection;

    public PlayerRelocateGameEvent playerRelocateGameEvent;

    public GameEvent onReturnGameLobby;

    public GameEvent onStartSTS;

    public GameEvent onStartUGS;

    public GameEvent onGameLobbyInitialized;

    public GameEvent onPlayerJoinedEvent;

    public SingleStringGameEvent onDisplayWarningText;

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
        minigameSequence = new List<GameStats.Scene>();
        playerObjects = new Dictionary<int, Transform>();
        minigameSelection = new Dictionary<GameStats.Scene, bool>();
        playerInputManager = PlayerInputManager.instance;
        gameStats.SetCurrentScene(GameStats.Scene.gameLobby);
        gameStats.tutorialModeOn = false;
        onGameLobbyInitialized.Fire();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        int playerID = PhotonNetwork.CurrentRoom.PlayerCount;
        GameObject player =
            PhotonNetwork
                .Instantiate(playerPrefab.name,
                GetPlayerLocation(playerID),
                playerPrefab.transform.rotation);
        playerObjects[playerID] = player.transform;
        PlayerStats newPlayerStats =
            SwitchPlayerProfile(playerID, PhotonNetwork.NickName); // assign one profile to the joined player
        players.AddPlayer (newPlayerStats, player);
        player.GetComponent<PlayerController>().EnableController();
        player.GetComponent<PlayerController>().EnableMovement();
        playerRelocateGameEvent.Fire(playerID, GetPlayerLocation(playerID));
        onPlayerJoinedEvent.Fire();
    }

    public void AddPlayer(GameObject player)
    {
        PlayerStats playerStats =
            player.GetComponent<PlayerStatsManager>().GetPlayerStats();
        int playerID = playerStats.playerID;
        Debug.Log(string.Format("Player {0} added!", playerID));
        playerObjects[playerID] = player.transform;
        players.AddPlayer (playerStats, player);
    }

    private Vector3 GetPlayerLocation(int playerID)
    {
        foreach (PlayerLocation playerLocation in playerLocations)
        {
            if (playerLocation.playerID == playerID)
            {
                return playerLocation.location.position;
            }
        }
        return Vector3.zero;
    }

    public void OnPlayerSwitchProfile(int playerID, string playerName)
    {
        Debug.Log(string.Format("Switching profile for Player {0}", playerID));
        PlayerStats newPlayerStats = SwitchPlayerProfile(playerID, playerName);
        players.UpdatePlayer (playerID, newPlayerStats);
    }

    // Switch a character for a given player
    // Will set the current player's character to be unselected, remove playerID
    // Choose the next available character to assign to this player
    public PlayerStats SwitchPlayerProfile(int playerID, string playerName)
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
                playerStats.playerName = "";
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
                playerStats.playerName = playerName;
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
            playerStats.ready = false;
            playerStats.coins = 0;
            playerStats.score = 0;
            playerStats.item = null;
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
            playerStats.masks = 0;
            playerStats.ready = false;
            playerStats.item = null;
            playerStats.playerName = "";
        }
    }

    // Call this to start the minigame
    public void OnStartMiniGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            onDisplayWarningText
                .Fire("Game cannot be started. Need at least 2 players.");
        }
        else
        {
            foreach (GameStats.Scene sceneType in minigameSelection.Keys)
            {
                if (minigameSelection[sceneType] == true)
                {
                    minigameSequence.Add (sceneType);
                }
            }
            if (minigameSequence.Count == 0)
            {
                onDisplayWarningText
                    .Fire("Game cannot be started. Need at least 1 game selected.");
                return;
            }
            minigameSequence.Add(GameStats.Scene.awardCeremony); // always have award ceremony at the end of the game
            gameStats.SetCurrentScene(minigameSequence[0]);
            currentMinigameSceneIdx = 0;
            GameStats.Scene firstScene = gameStats.GetCurrentScene();
            LoadMinigame (firstScene);
        }
    }

    public void LoadMinigame(GameStats.Scene sceneTag)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            switch (sceneTag)
            {
                case GameStats.Scene.gameLobby:
                    PhotonNetwork.LoadLevel("GameLobby");
                    break;
                case GameStats.Scene.swabTestWar:
                    PhotonNetwork.LoadLevel("SwabTestWar");
                    break;
                case GameStats.Scene.stopTheSpread:
                    onStartSTS.Fire();
                    PhotonNetwork.LoadLevel("StopTheSpread");
                    break;
                case GameStats.Scene.unlimitedGroupSize:
                    onStartUGS.Fire();
                    PhotonNetwork.LoadLevel("UnlimitedGroupSize");
                    break;
                case GameStats.Scene.snatchAndHoard:
                    PhotonNetwork.LoadLevel("Snatch&Hoard");
                    break;
                case GameStats.Scene.awardCeremony:
                    PhotonNetwork.LoadLevel("RewardCeremony");
                    break;
            }
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

    public void onMinigameSelected(GameStats.Scene minigameType)
    {
        minigameSelection[minigameType] = true;
    }

    public void onMinigameDeSelected(GameStats.Scene minigameType)
    {
        minigameSelection[minigameType] = false;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GameLobbyControlHandler : MonoBehaviour
{
    private int minigameSelectorEntered;

    private List<int> minigameIndexSelected;

    public GameEvent onMiniGameStart;

    public GameEvent onSwitchBGM;

    public SingleIntegerGameEvent onPlayerChangeProfile;

    public SingleIntegerGameEvent onMinigameSelected;

    public SingleIntegerGameEvent onMinigameDeSelected;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerAudioController playerAudioController;

    public GameStats gameStats;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
        playerAudioController = GetComponent<PlayerAudioController>();
        minigameIndexSelected = new List<int>(4);
        for (int i = 0; i < 4; i++)
        {
            minigameIndexSelected.Add (i);
        }
    }

    public void OnUse()
    {
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.clothChanger
        )
        {
            playerAudioController.PlaySFX(SFXType.changeOutfit);
            onPlayerChangeProfile
                .Fire(playerStatsManager.GetPlayerStats().playerID);
        }
        if (playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.reception)
        {
            onMiniGameStart.Fire();
        }
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.musicChanger
        )
        {
            onSwitchBGM.Fire();
        }
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.gameSelector
        )
        {
            if (minigameIndexSelected.Contains(minigameSelectorEntered))
            {
                onMinigameDeSelected.Fire (minigameSelectorEntered);
                minigameIndexSelected.Remove (minigameSelectorEntered);
            }
            else
            {
                onMinigameSelected.Fire (minigameSelectorEntered);
                minigameIndexSelected.Add (minigameSelectorEntered);
            }
        }
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.tutorialModeBtn
        )
        {
            gameStats.tutorialModeOn = !gameStats.tutorialModeOn;
        }
    }

    public void SetCurrentMinigameSelectorEntered(int index)
    {
        minigameSelectorEntered = index;
    }
}

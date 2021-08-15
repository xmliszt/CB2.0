using System.Collections.Generic;
using UnityEngine;

public class GameLobbyControlHandler : MonoBehaviour
{
    private int minigameSelectorEntered;

    private List<int> minigameIndexSelected;

    public GameEvent onMiniGameStart;

    public GameEvent onSwitchBGM;

    public IntegerStringGameEvent onPlayerChangeProfile;

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
                .Fire(playerStatsManager.GetPlayerStats().playerID, playerStatsManager.GetPlayerStats().playerName);
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
                playerAudioController.PlaySFX(SFXType.drop);
            }
            else
            {
                playerAudioController.PlaySFX(SFXType.shoot);
                onMinigameSelected.Fire (minigameSelectorEntered);
                minigameIndexSelected.Add (minigameSelectorEntered);
            }
        }
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.tutorialModeBtn
        )
        {
            if (gameStats.tutorialModeOn)
                playerAudioController.PlaySFX(SFXType.ready1);
            else
                playerAudioController.PlaySFX(SFXType.ready4);
            gameStats.tutorialModeOn = !gameStats.tutorialModeOn;
        }
    }

    public void SetCurrentMinigameSelectorEntered(int index)
    {
        minigameSelectorEntered = index;
    }
}

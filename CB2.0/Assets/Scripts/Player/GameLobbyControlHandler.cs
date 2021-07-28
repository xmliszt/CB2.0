using UnityEngine;

public class GameLobbyControlHandler : MonoBehaviour
{
    public GameEvent onMiniGameStart;

    public GameEvent onSwitchBGM;

    public SingleIntegerGameEvent onPlayerChangeProfile;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerAudioController playerAudioController;
    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
        playerAudioController = GetComponent<PlayerAudioController>();
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
    }
}

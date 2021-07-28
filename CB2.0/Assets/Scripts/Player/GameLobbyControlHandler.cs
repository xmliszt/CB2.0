using UnityEngine;

public class GameLobbyControlHandler : MonoBehaviour
{
    public GameEvent onMiniGameStart;

    public GameEvent onSwitchBGM;

    public SingleIntegerGameEvent onPlayerChangeProfile;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
    }

    public void OnUse()
    {
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.clothChanger
        )
        {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CeremonySkipper : MonoBehaviour
{
    public GameEvent onCeremonyEnd;

    public GameEvent playNextMinigame;

    public Players players;

    public GameObject playerImageHolderPanel;

    private Image[] exitedPlayerImages;

    private Dictionary<int, PlayerStats> exitedPlayers;

    private int exitCount = 0;

    private void Start()
    {
        exitedPlayerImages =
            playerImageHolderPanel.GetComponentsInChildren<Image>();
        exitedPlayers = new Dictionary<int, PlayerStats>();

        for (int i = 1; i <= 4; i++)
        {
            exitedPlayers[i] = null;
            exitedPlayerImages[i - 1].enabled = false;
        }

        foreach (KeyValuePair<int, PlayerInfo>
            playerInfo
            in
            players.GetPlayers()
        )
        {
            int playerID = playerInfo.Key;
            PlayerStats playerStats = playerInfo.Value.playerStats;

            exitedPlayerImages[playerID - 1].sprite = playerStats.playerAvatar;
        }
    }

    private void Update()
    {
        foreach (int playerID in players.GetPlayers().Keys)
        {
            int idx = playerID - 1;
            if (exitedPlayers[playerID] != null)
            {
                exitedPlayerImages[playerID - 1].enabled = true;
            }
        }
        int exitCount = 0;
        foreach (int playerID in exitedPlayers.Keys)
        {
            if (exitedPlayers[playerID] != null)
            {
                exitCount ++;
            }
        }

        if (exitCount == players.GetPlayers().Count)
        {
            StartCoroutine(startTransitionToLobby());
        }

    }

    private IEnumerator startTransitionToLobby()
    {
        yield return new WaitForSeconds(1.5f);

        // Exit ceremony!
        onCeremonyEnd.Fire();
        playNextMinigame.Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats _stats =
                other
                    .gameObject
                    .GetComponent<PlayerStatsManager>()
                    .GetPlayerStats();
            exitedPlayers[_stats.playerID] = _stats;
            other
                .gameObject
                .GetComponent<PlayerController>()
                .DisableController();
            other.gameObject.GetComponent<PlayerController>().DisableMovement();
        }
    }
}

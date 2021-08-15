using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPlayerController : MonoBehaviour
{
    public Players players;

    private Image[] playerReadyImages;

    private TMP_Text[] playerReadyText;

    private CanvasGroup[] playerReadyCG;

    private void Start()
    {
        playerReadyImages = GetComponentsInChildren<Image>();
        playerReadyText = GetComponentsInChildren<TMP_Text>();
        playerReadyCG = GetComponentsInChildren<CanvasGroup>();
        for (int i = 0; i < playerReadyCG.Length; i++)
        {
            playerReadyCG[i].alpha = 0;
        }
        foreach (KeyValuePair<int, PlayerInfo>
            playerInfo
            in
            players.GetPlayers()
        )
        {
            int idx = playerInfo.Key - 1;
            PlayerInfo _info = playerInfo.Value;
            playerReadyCG[idx].alpha = 0;
            playerReadyImages[idx].sprite = _info.playerStats.playerAvatar;
            playerReadyText[idx].text = _info.playerStats.playerName;
            playerReadyText[idx].color = _info.playerStats.playerAccent;
        }
    }

    private void Update()
    {
        foreach (KeyValuePair<int, PlayerInfo>
            playerInfo
            in
            players.GetPlayers()
        )
        {
            int idx = playerInfo.Key - 1;
            PlayerInfo _info = playerInfo.Value;
            if (_info.playerStats.ready)
            {
                playerReadyCG[idx].alpha = 1;
            }
        }
    }
}

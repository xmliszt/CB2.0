using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoEditor : MonoBehaviour
{
    public Image avatar;

    public TMP_Text rank;
    
    public TMP_Text playerID;

    public TMP_Text playerScore;
    private PlayerStats playerStats;
    public void SetPlayerStats(PlayerStats _playerStats)
    {
        playerStats = _playerStats;
        if (playerStats == null)
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 1;
            rank.color = playerStats.playerAccent;
            avatar.sprite = playerStats.playerAvatar;
            playerID.text = string.Format("{0}P", playerStats.playerID);
            playerID.color = playerStats.playerAccent;
            playerScore.text = playerStats.score.ToString();
        }
    }
}

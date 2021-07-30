using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoEditor : MonoBehaviour
{
    public Image avatar;

    public TMP_Text rank;
    
    public TMP_Text playerID;

    public TMP_Text playerScore;

    public TMP_Text playerMaskObtained;
    public TMP_Text playerCurrentTotalMasks;
    private PlayerStats playerStats;
    public void SetPlayerStats(PlayerStats _playerStats, int rewardToObtained)
    {
        playerStats = _playerStats;
        if (playerStats == null)
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 1;
            rank.text = playerStats.GetRank().ToString();
            rank.color = playerStats.playerAccent;
            avatar.sprite = playerStats.playerAvatar;
            playerID.text = string.Format("{0}P", playerStats.playerID);
            playerID.color = playerStats.playerAccent;
            playerScore.text = playerStats.score.ToString();
            playerScore.fontSize = 100 - (playerStats.GetRank() - 1) * 10;
            playerMaskObtained.text = "+" + rewardToObtained.ToString();
            playerMaskObtained.fontSize = 100 - (playerStats.GetRank() - 1) * 10;
            playerCurrentTotalMasks.text =playerStats.masks.ToString();
        }
    }
}

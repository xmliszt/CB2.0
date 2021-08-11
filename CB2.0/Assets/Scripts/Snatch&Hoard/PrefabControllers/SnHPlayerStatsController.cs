using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SnHPlayerStatsController : MonoBehaviour
{
    public int playerID;

    public Players players;
    public SnHGameConstants gameConstants;

    public Image avatar;
    public Image otherObject;
    public Text name;
    public Text coinsCollected;
    public Text TPcollected;
    public Text otherCollected;

    public List<Sprite> itemSprites;

    private PlayerStats uniquePlayerStats;

    // when the game is started or restarted

    private void Start() {
        uniquePlayerStats = players.GetPlayers()[playerID].playerStats;
    }
    public void onStart()
    {
        if (uniquePlayerStats.isActive) //this player is in use
        {
            // first update of images to be used
            avatar.sprite = uniquePlayerStats.playerAvatar;
            name.text = string.Format("{0}P", uniquePlayerStats.playerID);
            name.color = uniquePlayerStats.playerAccent;
            otherObject.sprite = itemSprites[gameConstants.OtherIndex];

            // reset all scores
            coinsCollected.color = uniquePlayerStats.playerAccent;
            TPcollected.color = uniquePlayerStats.playerAccent;
            otherCollected.color = uniquePlayerStats.playerAccent;
            coinsCollected.text = formatString(uniquePlayerStats.coins);
            TPcollected.text = formatString(uniquePlayerStats.TPCollected);
            otherCollected.text = formatString(uniquePlayerStats.otherObjectCollected);
        }
    }

    // anytime the number of coins, TP or others collected changes
    public void Update()
    {
        if (uniquePlayerStats.isActive)
        {
            coinsCollected.text = formatString(uniquePlayerStats.coins);
            TPcollected.text = formatString(uniquePlayerStats.TPCollected);
            otherCollected.text = formatString(uniquePlayerStats.otherObjectCollected);
        }
    }

    private string formatString(int score)
    {
        if (score.ToString().Length == 1)
        {
            return "0" + score.ToString();
        }
        else
        {
            return score.ToString();
        }
    }
}

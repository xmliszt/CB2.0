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

    private bool playerIsInGame = false;

    // when the game is started or restarted
    public void Initialise()
    {
        if (players.GetPlayers().ContainsKey(playerID))
        {
            GetComponent<Canvas>().enabled = true;
            uniquePlayerStats = players.GetPlayers()[playerID].playerStats;
            playerIsInGame = true;

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
            otherCollected.text =
                formatString(uniquePlayerStats.otherObjectCollected);
        }
        else
        {
            GetComponent<Canvas>().enabled = false;
        }
    }

    // anytime the number of coins, TP or others collected changes
    public void Update()
    {
        if (playerIsInGame)
        {
            coinsCollected.text = formatString(uniquePlayerStats.coins);
            TPcollected.text = formatString(uniquePlayerStats.TPCollected);
            otherCollected.text =
                formatString(uniquePlayerStats.otherObjectCollected);
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

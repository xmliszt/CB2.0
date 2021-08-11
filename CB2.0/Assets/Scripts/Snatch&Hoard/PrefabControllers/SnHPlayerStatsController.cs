using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SnHPlayerStatsController : MonoBehaviour
{
    public SnHPlayerStats uniquePlayerStats;
    public SnHGameConstants gameConstants;

    public Image avatar;
    public Image otherObject;
    public Text name;
    public Text coinsCollected;
    public Text TPcollected;
    public Text otherCollected;

    public List<Sprite> avatars;
    public List<Sprite> itemSprites;

    // when the game is started or restarted
    public void onStart()
    {
        if (uniquePlayerStats.isActive) //this player is in use
        {
            // first update of images to be used
            avatar.sprite = avatars[uniquePlayerStats.playerAvatar];
            name.text = uniquePlayerStats.playerName;
            otherObject.sprite = itemSprites[gameConstants.OtherIndex];

            // reset all scores
            coinsCollected.text = formatString(uniquePlayerStats.coinsCollected);
            TPcollected.text = formatString(uniquePlayerStats.TPCollected);
            otherCollected.text = formatString(uniquePlayerStats.otherObjectCollected);
        }
    }

    // anytime the number of coins, TP or others collected changes
    public void Update()
    {
        if (uniquePlayerStats.isActive)
        {
            coinsCollected.text = formatString(uniquePlayerStats.coinsCollected);
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

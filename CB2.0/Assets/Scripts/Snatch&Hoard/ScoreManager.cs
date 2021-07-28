using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public SnHGameConstants snHGameConstants;

    public Text CollectTP;
    public Image otherImage;
    public Text CollectOther;
    public GameObject P1StatsDisplay;
    public GameObject P2StatsDisplay;
    public GameObject P3StatsDisplay;
    public GameObject P4StatsDisplay;
    public SpriteRenderer P1Background;
    public SpriteRenderer P2Background;
    public SpriteRenderer P3Background;
    public SpriteRenderer P4Background;
    
    public List<Sprite> others;

    public void onStart()
    {
        // set the Collect values
        otherImage.sprite = others[snHGameConstants.OtherIndex];
        CollectTP.text = formatString(snHGameConstants.CollectTP);
        CollectOther.text = formatString(snHGameConstants.CollectOther);

        // set active/inactive players
        StartPlayerStatsBackground();
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

    private void StartPlayerStatsBackground()
    {
        if (snHGameConstants.P1Playing)
        {
            PlayerActive(P1StatsDisplay, P1Background);
        }
        else
        {
            PlayerInactive(P1StatsDisplay, P1Background);
        }

        if (snHGameConstants.P2Playing)
        {
            PlayerActive(P2StatsDisplay, P2Background);
        }
        else
        {
            PlayerInactive(P2StatsDisplay, P2Background);
        }

        if (snHGameConstants.P3Playing)
        {
            PlayerActive(P3StatsDisplay, P3Background);
        }
        else
        {
            PlayerInactive(P3StatsDisplay, P3Background);
        }

        if (snHGameConstants.P4Playing)
        {
            PlayerActive(P4StatsDisplay, P4Background);
        }
        else
        {
            PlayerInactive(P4StatsDisplay, P4Background);
        }
    }

    private void PlayerActive(GameObject display, SpriteRenderer background)
    {
        background.color = snHGameConstants.activeColour;
        display.SetActive(true);
        display.GetComponent<SnHPlayerStatsController>().onStart();
    }

    private void PlayerInactive(GameObject display, SpriteRenderer background)
    {
        background.color = snHGameConstants.inactiveColour;
        display.SetActive(false);
    }
}

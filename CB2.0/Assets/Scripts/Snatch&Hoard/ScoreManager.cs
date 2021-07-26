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
    public SnHPlayerStatsController P1StatsController;
    public SnHPlayerStatsController P2StatsController;
    public SnHPlayerStatsController P3StatsController;
    public SnHPlayerStatsController P4StatsController;
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
            PlayerActive(P1StatsController, P1Background);
        }
        else
        {
            PlayerInactive(P1StatsController, P1Background);
        }

        if (snHGameConstants.P2Playing)
        {
            PlayerActive(P2StatsController, P2Background);
        }
        else
        {
            PlayerInactive(P2StatsController, P2Background);
        }

        if (snHGameConstants.P3Playing)
        {
            PlayerActive(P3StatsController, P3Background);
        }
        else
        {
            PlayerInactive(P3StatsController, P3Background);
        }

        if (snHGameConstants.P4Playing)
        {
            PlayerActive(P4StatsController, P4Background);
        }
        else
        {
            PlayerInactive(P4StatsController, P4Background);
        }
    }

    private void PlayerActive(SnHPlayerStatsController controller, SpriteRenderer background)
    {
        background.color = snHGameConstants.activeColour;
        controller.enabled = true;
        controller.onStart();
    }

    private void PlayerInactive(SnHPlayerStatsController controller, SpriteRenderer background)
    {
        background.color = snHGameConstants.inactiveColour;
        controller.enabled = false;
    }
}

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

    public List<Sprite> itemSprites;
    
    public void onStart()
    {
        // set the Collect values
        otherImage.sprite = itemSprites[snHGameConstants.OtherIndex];
        CollectTP.text = formatString(snHGameConstants.CollectTP);
        CollectOther.text = formatString(snHGameConstants.CollectOther);
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

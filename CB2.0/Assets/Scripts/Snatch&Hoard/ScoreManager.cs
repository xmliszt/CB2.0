using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{

    public GameEvent onOtherItemChosen;
    public SnHGameConstants snHGameConstants;

    public Text CollectTP;
    public Image otherImage;
    public Text CollectOther;

    public List<Sprite> itemSprites;

    private void Start() {
        // set the Collect values
        // select random item for game constant
        int otheritem = Random.Range(0, 3); // game constant have to follow pickup index
        snHGameConstants.OtherIndex = otheritem + 3;
        onOtherItemChosen.Fire();

        int TPCollected = Random.Range(snHGameConstants.collectTotal / 2 - 2, snHGameConstants.collectTotal / 2 + 3);
        int remaining = snHGameConstants.collectTotal - TPCollected;

        snHGameConstants.CollectTP = TPCollected;
        snHGameConstants.CollectOther = remaining;

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

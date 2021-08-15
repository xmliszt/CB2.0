using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnlimitedGroupSizeUIUpdater : MonoBehaviour
{
    public PlayerStats playerStats;
    
    public int playerID;

    public Players players;

    public TMP_Text scoreText;

    public TMP_Text completeTestText;

    public TMP_Text playerIndicatorText;

    public Image playerAvatar;

    public Image panelBackground;

    // private PlayerStats playerStats;

    private BoxCollider2D boxCollider2D;

    private bool _enabled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled)
        {
            scoreText.text = string.Format("Coins: {0}", playerStats.coins);
            completeTestText.text =
                string.Format("NPC: {0}", playerStats.score);
        }
    }
}

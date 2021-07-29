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
        // GetComponent<CanvasGroup>().alpha = 1;
        // if (players.GetPlayers().ContainsKey(playerID))
        // {
        //     Debug.Log("UI START");
            
        //     playerStats = players.GetPlayers()[playerID].playerStats;

        //     _enabled = true;

        //     // Set the size of trigger;
        //     RectTransform rt = transform.GetComponent<RectTransform>();
        //     float width = rt.sizeDelta.x;
        //     float height = rt.sizeDelta.y;
        //     boxCollider2D = GetComponent<BoxCollider2D>();
        //     boxCollider2D.size = new Vector2(width, height);

        //     // set player indicator
        //     playerIndicatorText.text =
        //         string.Format("{0}P", playerStats.playerID);
        //     playerIndicatorText.color = playerStats.playerAccent;

        //     // set avatar
        //     playerAvatar.sprite = playerStats.playerAvatar;

        //     // set background color
        //     panelBackground.color = playerStats.playerAccent;
        //     Color tmp = panelBackground.color;
        //     panelBackground.color = new Color(tmp.r, tmp.g, tmp.b, 0.3f);
        // }
        // else
        // {
        //     _enabled = false;
        //     GetComponent<CanvasGroup>().alpha = 0;
        // }
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

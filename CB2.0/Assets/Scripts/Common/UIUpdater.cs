using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text completeTestText;

    public TMP_Text playerIndicatorText;

    public Image playerAvatar;
    public Image panelBackground;

    public PlayerStats playerStats;

    private BoxCollider2D boxCollider2D;

    private void Start() {
        // Set the size of trigger;
        RectTransform rt = transform.GetComponent<RectTransform>();
        float width = rt.sizeDelta.x;
        float height = rt.sizeDelta.y;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(width, height);

        // set player indicator
        playerIndicatorText.text = string.Format("{0}P", playerStats.playerID);
        playerIndicatorText.color = playerStats.playerAccent;

        // set avatar
        playerAvatar.sprite = playerStats.playerAvatar;
        
        // set background color
        panelBackground.color = playerStats.playerAccent;
        Color tmp = panelBackground.color;
        panelBackground.color = new Color(tmp.r, tmp.g, tmp.b, 0.3f);
    }

    void Update()
    {
        scoreText.text = string.Format("Coins: {0}", playerStats.coins);
        completeTestText.text = string.Format("Swab Test Done: {0}", playerStats.score);
    }
}

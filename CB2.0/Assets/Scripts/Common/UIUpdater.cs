using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text completeTestText;

    public Image playerAvatar;
    public Image panelBackground;

    public PlayerStats playerStats;

    private void Start() {
        playerAvatar.sprite = playerStats.playerAvatar;
        panelBackground.color = playerStats.playerAccent;
        Color tmp = panelBackground.color;
        panelBackground.color = new Color(tmp.r, tmp.g, tmp.b, 0.5f);
    }

    void Update()
    {
        scoreText.text = string.Format("Coins: {0}", playerStats.coins);
        completeTestText.text = string.Format("Swab Test Done: {0}", playerStats.completedSwabTests);
    }


    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Color tmp = panelBackground.color;
            panelBackground.color = new Color(tmp.r, tmp.g, tmp.b, 0.1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Color tmp = panelBackground.color;
            panelBackground.color = new Color(tmp.r, tmp.g, tmp.b, 0.5f);
        }
    }
}

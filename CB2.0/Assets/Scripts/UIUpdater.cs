using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text completeTestText;

    public PlayerStats playerStats;

    void Update()
    {
        scoreText.text = string.Format("Score: {0}", playerStats.coins);
        completeTestText.text = string.Format("Swab Test Done: {0}", playerStats.completedSwabTests);
    }
}

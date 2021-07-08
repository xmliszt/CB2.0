using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RankComparer : IComparer<PlayerStats>
{
    public int Compare(PlayerStats a, PlayerStats b)
    {
        return -1 * a.score.CompareTo(b.score); // descending order
    }
}

[System.Serializable]
public class RankHandler : MonoBehaviour
{
    public Players players;

    public GameObject playerInfoPanel;

    private CanvasGroup[] playerInfoRows;

    private PlayerInfoEditor[] playerInfoEditors;

    private CanvasGroup gameoverUI;

    private List<PlayerStats> playerStatsList;

    private void Start() {
        playerStatsList = new List<PlayerStats>();
        gameoverUI = GetComponent<CanvasGroup>();
        gameoverUI.alpha = 0;
        playerInfoRows = playerInfoPanel.GetComponentsInChildren<CanvasGroup>();
        playerInfoEditors = playerInfoPanel.GetComponentsInChildren<PlayerInfoEditor>();
        foreach(KeyValuePair<int, PlayerInfo> player in players.GetPlayers())
        {
            PlayerStats playerStats = player.Value.playerStats;
            playerStatsList.Add(playerStats);
        }
        foreach (CanvasGroup playerInfoRow in playerInfoRows)
        {
            playerInfoRow.alpha = 0;
        }
    }
    
    private void Rank()
    {
        RankComparer comparer = new RankComparer();
        playerStatsList.Sort(comparer);

        int _rank = 1;
        playerStatsList[0].SetRank(_rank);
        _rank = 2;
        for (int i = 1; i < playerStatsList.Count; i ++)
        {
            if (playerStatsList[i].score == playerStatsList[i-1].score)
            {
                playerStatsList[i].SetRank(playerStatsList[i-1].GetRank());
            } else {
                playerStatsList[i].SetRank(_rank);
            }
            _rank ++;
        }
    }

    public void ShowGameOver()
    {
        Rank();
        StartCoroutine(Fade(gameoverUI, 0, 1, 1));
        StartCoroutine(ShowRanks());
    }
    
    private IEnumerator Fade(CanvasGroup group, float from, float to, float duration) {
        float elapsedTime = 0f;
        while (elapsedTime <= duration) {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }
        group.alpha = to;
    }

    private IEnumerator ShowRanks()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < playerStatsList.Count; i ++)
        {
            playerInfoEditors[i].SetPlayerStats(playerStatsList[i]);
            StartCoroutine(Fade(playerInfoRows[i], 0, 1, 1f));
            yield return new WaitForSeconds(0.5f);
        }
    }
}

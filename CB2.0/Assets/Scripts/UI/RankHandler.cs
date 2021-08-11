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

public enum Rank {
    rank1 = 1,
    rank2 = 2,
    rank3 = 3,
    rank4 = 4,
}

[System.Serializable]
public class MaskReward {
    public Rank _rank;
    public int maskRewarded;
}

[System.Serializable]
public class RankHandler : MonoBehaviour
{
    public List<MaskReward> maskRewardSetting = new List<MaskReward>(4);
    public GameConstants gameConstants;
    public Players players;

    public GameObject playerInfoPanel;

    public GameEvent onPlayNextMiniGame;

    private CanvasGroup[] playerInfoRows;

    private PlayerInfoEditor[] playerInfoEditors;

    private CanvasGroup gameoverUI;

    private List<PlayerStats> playerStatsList;

    private List<int> masksRewardedList;
    private void Start() {
        playerStatsList = new List<PlayerStats>();
        masksRewardedList = new List<int>(players.GetPlayers().Count);
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
        playerStatsList[0].masks += maskRewardSetting[_rank-1].maskRewarded;
        masksRewardedList.Add(maskRewardSetting[_rank-1].maskRewarded);
        
        _rank = 2;
        for (int i = 1; i < playerStatsList.Count; i ++)
        {
            if (playerStatsList[i].score == playerStatsList[i-1].score)
            {
                playerStatsList[i].SetRank(playerStatsList[i-1].GetRank());
                playerStatsList[i].masks += maskRewardSetting[playerStatsList[i-1].GetRank()-1].maskRewarded;
                masksRewardedList.Add(maskRewardSetting[playerStatsList[i-1].GetRank()-1].maskRewarded);
            } else {
                playerStatsList[i].SetRank(_rank);
                playerStatsList[i].masks += maskRewardSetting[_rank-1].maskRewarded;
                masksRewardedList.Add(maskRewardSetting[_rank-1].maskRewarded);
            }
            _rank ++;
        }
    }

    public void ShowGameOver()
    {
        Rank();
        StartCoroutine(Fade(gameoverUI, 0, 1, 1));
        StartCoroutine(ShowRanks());
        StartCoroutine(PlayNextGame());
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
            playerInfoEditors[i].SetPlayerStats(playerStatsList[i], masksRewardedList[i]);
            StartCoroutine(Fade(playerInfoRows[i], 0, 1, 1f));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator PlayNextGame() {
        yield return new WaitForSeconds(gameConstants.nextGameDelay);
        onPlayNextMiniGame.Fire();
    }
}

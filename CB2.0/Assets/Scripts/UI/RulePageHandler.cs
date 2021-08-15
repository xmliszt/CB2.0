using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RulePageHandler : MonoBehaviour
{
    public GameEvent onReady;

    public Players players;

    private VideoPlayer rulePageVideoPlayer;

    private bool isWaitingForPlayer = false;

    private bool isPaused = false;

    private void Awake()
    {
        rulePageVideoPlayer = GetComponent<VideoPlayer>();
        rulePageVideoPlayer.Prepare();
    }

    public void PauseTime()
    {
        rulePageVideoPlayer.Stop();
        rulePageVideoPlayer.Play();
        isPaused = !isPaused;
    }

    public void ResumeTime()
    {
        rulePageVideoPlayer.Stop();
        isPaused = false;
    }

    public void StartWaitForPlayer()
    {
        isWaitingForPlayer = true;
    }

    private bool isEveryoneReady()
    {
        int readyCount = 0;
        foreach (KeyValuePair<int, PlayerInfo> player in players.GetPlayers())
        {
            if (player.Value.playerStats.ready)
            {
                readyCount++;
            }
        }
        return readyCount == players.GetPlayers().Count;
    }

    private void Update()
    {
        if (isWaitingForPlayer)
        {
            if (isEveryoneReady())
            {
                isWaitingForPlayer = false;
                ResumeTime();
                StartCoroutine(StartReady());
            }
        }
    }

    IEnumerator StartReady()
    {
        yield return new WaitForSeconds(1);
        onReady.Fire();
    }
}

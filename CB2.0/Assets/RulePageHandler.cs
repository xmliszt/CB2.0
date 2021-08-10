using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulePageHandler : MonoBehaviour
{
    public GameEvent onReady;

    public Players players;

    private Image rulePageImage;

    private bool isWaitingForPlayer = false;

    private void Start()
    {
        rulePageImage = GetComponent<Image>();
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1;
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
                readyCount ++;
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

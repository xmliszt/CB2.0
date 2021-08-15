using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public GameEvent OnGameOver;

    public GameEvent OnBGMPitchUp;

    public GameEvent OnBGMPitchDown;

    public GameConstants constants;

    public TMP_Text countdownText;

    public Color initializedColor;

    [Header("Last 10 seconds warning color")]
    public Color lastTenSecondsColor;

    private CanvasGroup canvas;

    private int totalSeconds;

    private bool isPaused = false;

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        ResetTimer();
    }

    public void StartCountdown()
    {
        ResetTimer();
        canvas.alpha = 1;
        StartCoroutine(RunTimer());
    }

    public void PauseCountdown()
    {
        isPaused = !isPaused;
    }

    IEnumerator RunTimer()
    {
        while (totalSeconds > 0)
        {
            if (!isPaused)
            {
                yield return new WaitForSeconds(1);
                totalSeconds--;
                if (totalSeconds <= 10)
                {
                    OnBGMPitchUp.Fire();
                    countdownText.color = lastTenSecondsColor;
                }
                ShowTime();
            }
            else
            {
                yield return null;
            }
        }
        OnBGMPitchDown.Fire();
        OnGameOver.Fire();
        ResetTimer();
    }

    private void ResetTimer()
    {
        totalSeconds = constants.minigameCountdownTime;
        countdownText.color = initializedColor;
        canvas.alpha = 0;
    }

    private void ShowTime()
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        string minuteString = minutes.ToString().PadLeft(2, '0');
        string secondString = seconds.ToString().PadLeft(2, '0');
        string formattedText = minuteString + ":" + secondString;
        countdownText.text = formattedText;
    }
}

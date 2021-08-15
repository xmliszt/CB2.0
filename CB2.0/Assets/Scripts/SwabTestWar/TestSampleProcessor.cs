using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSampleProcessor : MonoBehaviour
{
    public GameConstants constants;

    public TestStation testStationInfo;

    public SpriteRenderer indicatorRenderer;

    public SpriteRenderer lockIndicator;

    public SpriteRenderer avatarRenderer;

    public Sprite waitingSprite;

    public Sprite completeSprite;

    private bool isPaused = false;

    private void Start()
    {
        indicatorRenderer.enabled = false;
        lockIndicator.enabled = false;
        avatarRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int playerID = other.transform.GetInstanceID();
            if (!testStationInfo.playersInZone.Contains(playerID))
            {
                testStationInfo.playersInZone.Add (playerID);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int playerID = other.transform.GetInstanceID();
            if (testStationInfo.playersInZone.Contains(playerID))
            {
                testStationInfo.playersInZone.Remove (playerID);
            }
        }
    }

    public void OnLoadTestSample(int playerID)
    {
        if (
            testStationInfo.playersInZone.Contains(playerID) &&
            !testStationInfo.isLoaded
        )
        {
            indicatorRenderer.enabled = true;
            indicatorRenderer.sprite = waitingSprite;
            testStationInfo.isLoaded = true;
            StartCoroutine(CountDownTimer(constants.countdownDuration));
        }
    }

    public void PauseTimer()
    {
        isPaused = !isPaused;
    }

    IEnumerator CountDownTimer(float duration)
    {
        int counter = 0;
        while (counter < duration)
        {
            if (isPaused)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(1);
                counter++;
            }
        }
        indicatorRenderer.sprite = completeSprite;
        testStationInfo.isComplete = true;
    }

    public void OnRetrieveTestResult(int playerID)
    {
        if (testStationInfo.playersInZone.Contains(playerID))
        {
            if (testStationInfo.isLocked)
            {
                if (playerID == testStationInfo.resultOwner)
                {
                    clearResult();
                    lockIndicator.enabled = false;
                    avatarRenderer.enabled = false;
                }
            }
            else
            {
                clearResult();
            }
        }
    }

    private void clearResult()
    {
        testStationInfo.isComplete = false;
        testStationInfo.isLocked = false;
        testStationInfo.isLoaded = false;
        indicatorRenderer.enabled = false;
        testStationInfo.resultOwner = 0;
    }

    public void OnLock(GameObject player)
    {
        testStationInfo.isLocked = true;
        testStationInfo.resultOwner = player.transform.GetInstanceID();
        Sprite playerAvatar =
            player
                .GetComponent<PlayerStatsManager>()
                .GetPlayerStats()
                .playerAvatar;
        avatarRenderer.sprite = playerAvatar;
        lockIndicator.enabled = true;
        avatarRenderer.enabled = true;
    }
}

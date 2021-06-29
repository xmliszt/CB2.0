using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TestSampleProcessor : MonoBehaviour
{
    public GameConstants constants;

    public TestStationInfoGameEvent infoCheckBeforeSubmitTestSample;

    public TestStationInfoGameEvent infoCheckBeforeResultRetrieval;

    public TestStation testStationInfo;

    public SpriteRenderer indicatorRenderer;

    public Sprite waitingSprite;

    public Sprite completeSprite;

    private void Start()
    {
        indicatorRenderer.enabled = false;
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
            testStationInfo.resultOwner.Add(playerID);
            StartCoroutine(CountDownTimer(constants.countdownDuration));
        }
    }

    IEnumerator CountDownTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        indicatorRenderer.sprite = completeSprite;
        testStationInfo.isComplete = true;
    }

    public void OnRetrieveTestResult(int playerID)
    {
        if (testStationInfo.playersInZone.Contains(playerID))
        {
            if (testStationInfo.isLocked)
            {
                if (playerID == testStationInfo.resultOwner[0])
                {
                    clearResult();
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
        testStationInfo.resultOwner = new List<int>();
    }

    public void ReturnTestStationInfoBeforeSubmitTestSample(int playerID)
    {
        if (testStationInfo.playersInZone.Contains(playerID))
        {
            infoCheckBeforeSubmitTestSample.Fire (testStationInfo);
        }
    }

    public void ReturnTestStationInfoBeforeResultRetrieval(int playerID)
    {
        if (testStationInfo.playersInZone.Contains(playerID))
        {
            infoCheckBeforeResultRetrieval.Fire (testStationInfo);
        }
    }
}

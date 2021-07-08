using System.Collections;
using UnityEngine;

public class ReadyStart : MonoBehaviour
{

    public GameObject readyText;
    public GameObject startText;

    public GameEvent OnCountDownStart;

    private int readyStartInterval = 2; // second

    public void PlayReadyStart()
    {
        Reset();
        StartCoroutine(Play());
        Reset();
    }
    
    IEnumerator Play()
    {
        yield return new WaitForSeconds(1);
        readyText.SetActive(true);
        yield return new WaitForSeconds(readyStartInterval);
        startText.SetActive(true);
        OnCountDownStart.Fire();
    }

    private void Reset()
    {
        readyText.SetActive(false);
        startText.SetActive(false);
    }
}

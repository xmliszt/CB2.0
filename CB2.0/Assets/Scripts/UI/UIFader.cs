using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    private CanvasGroup group;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (
                other
                    .gameObject
                    .GetComponent<PlayerStatsManager>()
                    .GetPlayerStats()
                    .playerID ==
                GetComponent<UIUpdater>().playerID
            ) StartCoroutine(FadeCGAlpha(1, 0.2f, 0.3f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (
                other
                    .gameObject
                    .GetComponent<PlayerStatsManager>()
                    .GetPlayerStats()
                    .playerID ==
                GetComponent<UIUpdater>().playerID
            ) StartCoroutine(FadeCGAlpha(0.2f, 1f, 0.3f));
        }
    }

    private IEnumerator FadeCGAlpha(float from, float to, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }
        group.alpha = to;
    }
}

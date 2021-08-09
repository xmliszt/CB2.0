using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningTextController : MonoBehaviour
{
    private CanvasGroup group;
    public TMP_Text warningText;

    private void Start() {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }
    private IEnumerator Fade(float from, float to, float duration)
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

    private void FadeOut()
    {
        StartCoroutine(Fade(1, 0, 1));
    }

    public void DisplayWarning(string text)
    {
        warningText.text = text;
        StartCoroutine(Fade(0, 1, 0.3f));
        Invoke("FadeOut", 5);
    }
}

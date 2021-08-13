using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulePageDisplayHandler : MonoBehaviour
{
    private bool isPaused = false;
    private bool hasShownRule = false;

    private void Update() {
        if (isPaused && !hasShownRule)
        {
            GetComponent<Canvas>().enabled = true;
            hasShownRule = true;
        }
        if (!isPaused && hasShownRule)
        {
            GetComponent<Canvas>().enabled = false;
            hasShownRule = false;
        }
    }

    public void Pause() {
        isPaused = !isPaused;
    }
}

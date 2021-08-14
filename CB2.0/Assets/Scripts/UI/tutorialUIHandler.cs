using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialUIHandler : MonoBehaviour
{
    public GameStats gameStats;

    public GameObject onBtn;

    public GameObject offBtn;

    private void Start() {
        onBtn.SetActive(false);
        offBtn.SetActive(false);
    }

    void Update()
    {
        if (gameStats.tutorialModeOn)
        {
            onBtn.SetActive(true);
            offBtn.SetActive(false);
        } else {
            onBtn.SetActive(false);
            offBtn.SetActive(true);
        }
    }
}

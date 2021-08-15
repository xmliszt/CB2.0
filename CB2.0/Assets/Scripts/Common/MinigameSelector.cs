using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSelector : MonoBehaviour
{
    public MinigameTagGameEvent onMinigameSelected;

    public MinigameTagGameEvent onMinigameDeselected;

    public Minigame[] minigameList;

    private Image[] minigameImageList;

    private EnterSelectionPanelDetection[] minigameScriptList;

    private void Awake()
    {
        minigameImageList = GetComponentsInChildren<Image>();
        minigameScriptList =
            GetComponentsInChildren<EnterSelectionPanelDetection>();
    }

    private void Start()
    {
        StartCoroutine(InitializeMinigameSelectors());
    }

    private IEnumerator InitializeMinigameSelectors()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < minigameList.Length; i++)
        {
            minigameScriptList[i].SetMinigameIndex(i);
            if (i == 0)
            {
                minigameImageList[i].sprite =
                    minigameList[i].minigameSelectedSprite;
                onMinigameSelected.Fire(minigameList[i].minigameType);
            }
            else
            {
                minigameImageList[i].sprite =
                    minigameList[i].minigameDeselectedSprite;
            }
        }
    }

    public void SelectMinigame(int minigameIndex)
    {
        if (minigameIndex == 0)
        {
            minigameImageList[minigameIndex].sprite =
                minigameList[minigameIndex].minigameSelectedSprite;
            onMinigameSelected.Fire(minigameList[minigameIndex].minigameType);
        }
    }

    public void DeselectMinigame(int minigameIndex)
    {
        if (minigameIndex == 0)
        {
            minigameImageList[minigameIndex].sprite =
                minigameList[minigameIndex].minigameDeselectedSprite;
            onMinigameDeselected.Fire(minigameList[minigameIndex].minigameType);
        }
    }
}

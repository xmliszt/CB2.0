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

    private void Start() {
       minigameImageList = GetComponentsInChildren<Image>();
       minigameScriptList = GetComponentsInChildren<EnterSelectionPanelDetection>();
       for (int i = 0; i < minigameList.Length; i ++)
       {
           minigameScriptList[i].SetMinigameIndex(i);
           minigameImageList[i].sprite = minigameList[i].minigameSelectedSprite;
           onMinigameSelected.Fire(minigameList[i].minigameType);
       }
    }

    public void SelectMinigame(int minigameIndex) {
        minigameImageList[minigameIndex].sprite = minigameList[minigameIndex].minigameSelectedSprite;
        onMinigameSelected.Fire(minigameList[minigameIndex].minigameType);
    }

    public void DeselectMinigame(int minigameIndex) {
        minigameImageList[minigameIndex].sprite = minigameList[minigameIndex].minigameDeselectedSprite;
        onMinigameDeselected.Fire(minigameList[minigameIndex].minigameType);
    }
}

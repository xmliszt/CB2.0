using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Minigame", fileName = "Minigame", order = 1)]
public class Minigame : ScriptableObject
{
    public GameStats.Scene minigameType;
    public Sprite minigameSelectedSprite;
    public Sprite minigameDeselectedSprite;
}

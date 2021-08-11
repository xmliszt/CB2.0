using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "SnHPlayerStats", menuName = "ScriptableObjects/SnHPlayerStats", order = 1)]
public class SnHPlayerStats : ScriptableObject
{
    // unique player information
    public int playerID;
    
    public string playerName;
    public Color playerAccent;
    public int playerAvatar;

    // animation controller
    public RuntimeAnimatorController animatorController;

    // collectible items
    public int coinsCollected;
    

    // player speed
    public float PlayerSpeed; // curerntly unused
    public float SlowMultiplier = 0.9f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "SnHPlayerStats", menuName = "ScriptableObjects/SnHPlayerStats", order = 1)]
public class SnHPlayerStats : ScriptableObject
{
    // unique player information
    public int playerID;
    public bool isActive;
    public string playerName;
    public Color playerAccent;
    public int playerAvatar;

    // animation controller
    public RuntimeAnimatorController animatorController;

    // collectible items
    public int coinsCollected;
    public int TPCollected;
    public int otherObjectCollected;

    // player speed
    public float playerSlow;
    public float SlowMultiplier = 0.9f;

    public enum ZoneType
    {
        pickUpZone = 0,
        myBasketZone = 1, // handled
        otherBasketZone = 2, // handled
        VMZone = 3,
        NPCZone = 4,
        CheckoutZone = 5,
        NotInAnyZone = 6
    }

    public ZoneType zoneType;
}

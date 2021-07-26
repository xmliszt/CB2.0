using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "SnHPickUps",
        menuName = "ScriptableObjects/SnHPickUps",
        order = 4)
]
public class SnHPickUps : ScriptableObject
{
    public enum PickUpType
    {
        coin = 0,
        toiletPaper = 1,
        chicken = 2,
        fish = 3, 
        cherry = 4,
        broccoli = 5,
        shopItem = 6,
        noneType = 7
    }

    public PickUpType pickUpType;
}

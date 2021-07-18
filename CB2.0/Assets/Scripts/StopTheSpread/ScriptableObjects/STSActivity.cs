using UnityEngine;

[
    CreateAssetMenu(
        fileName = "STSActivity",
        menuName = "ScriptableObjects/STS/Activity",
        order = 2)
]

public class STSActivity : ScriptableObject
{
    public enum STSActivityType
    {
        gym = 1,
        computer = 2,
        karaoke = 3,
        toiletPaper = 4,
        doingNothing = 5
    }

    public STSActivityType stsActivityType;

    public Sprite thoughtBubbleSprite;
}

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
        birthday = 5,
        doingNothing = 6
    }

    public STSActivityType stsActivityType;

    public Sprite thoughtBubbleSprite;
}

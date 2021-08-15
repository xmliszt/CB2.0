using UnityEngine;

[
    CreateAssetMenu(
        fileName = "PlayerStats",
        menuName = "ScriptableObjects/PlayerStats",
        order = 2)
]
public class PlayerStats : ScriptableObject
{
    public int statsID;
    public int playerID;

    public string playerName;

    public bool selected; // is this character profile selected by a player?

    public Color playerAccent;

    public Sprite playerAvatar;

    public RuntimeAnimatorController animatorController;

    public Item item;

    public int coins;

    public int score;

    public int masks; // overall ranked value

    public bool ready; // player ready to proceed from rule page

    private int rank;

    public int TPCollected;

    public int otherObjectCollected;

    public int GetRank()
    {
        return rank;
    }

    public void SetRank(int _rank)
    {
        rank = _rank;
    }

     public enum ZoneType
    {
        pickUpZone = 0, // handled
        myBasketZone = 1, // handled
        otherBasketZone = 2, // handled
        VMZone = 3,
        NPCZone = 4,
        CheckoutZone = 5,
        NotInAnyZone = 6,
        shopZone = 7,
    }

    public ZoneType zoneType;
}

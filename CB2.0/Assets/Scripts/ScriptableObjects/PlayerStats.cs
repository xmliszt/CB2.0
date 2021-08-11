using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    public PlayerInventory()
    {
        item = null;
    }

    public Item item;

    public Item GetCurrentItem()
    {
        return item;
    }

    public void SetItem(Item _item)
    {
        item = _item;
    }

    public void ClearItem()
    {
        item = null;
    }

    public Item useItem()
    {
        Item _item = item;
        item = null;
        return _item;
    }

    public bool hasItem()
    {
        return item != null;
    }
}

[
    CreateAssetMenu(
        fileName = "PlayerStats",
        menuName = "ScriptableObjects/PlayerStats",
        order = 2)
]
public class PlayerStats : ScriptableObject
{
    public int playerID;

    public string playerName;

    public bool selected; // is this character profile selected by a player?

    public Color playerAccent;

    public Sprite playerAvatar;

    public RuntimeAnimatorController animatorController;

    public PlayerInventory inventory = new PlayerInventory();

    public int coins;

    public int score;

    public int masks; // overall ranked value

    public bool ready; // player ready to proceed from rule page

    private int rank;

    public bool isActive = false;

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

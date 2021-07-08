using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    public ShopItem shopItem;

    public SpriteRenderer indicator;

    private void Start() {
        indicator.sprite = shopItem.itemSprite;
    }

    public ShopItem BuyItem(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStatsManager>().GetPlayerStats();
        
        if (playerStats.coins >= shopItem.cost)
        {
            playerStats.coins -= shopItem.cost;
            return shopItem;
        } else 
        {
            return null;
        }
    }
}

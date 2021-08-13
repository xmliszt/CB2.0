using UnityEngine;
using TMPro;

public class ShopHandler : MonoBehaviour
{
    public ShopItem shopItem;

    public TMP_Text coinTxt;

    public SpriteRenderer indicator;

    private void Start() {
        indicator.sprite = shopItem.itemSprite;
        coinTxt.text = shopItem.cost.ToString();
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

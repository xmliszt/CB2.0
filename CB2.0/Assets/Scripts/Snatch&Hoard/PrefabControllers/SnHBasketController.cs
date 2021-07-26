using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// no manager
public class SnHBasketController : MonoBehaviour
{
    public SnHPlayerStats snhPlayerStats;
    public SnHGameConstants gameConstants;

    // belongs to
    public int belongsToPlayer;

    // is player engaged with basket now
    public bool engagedWithAnyPlayer;
    public int engagedWithPlayer;

    // can be stolen from
    public bool canBeStolenFromBool;

    // basket status
    public SpriteRenderer Basket;

    // to switch on and off when another player is nearby
    public GameObject Unselected;
    public GameObject OwnedSelected;
    public GameObject NotOwnedSelected;

    // unselected
    public SpriteRenderer UnselectedAvatar;

    // owned and selected
    public SpriteRenderer OwnedSelectedAvatar;
    public SpriteRenderer TPBackground;
    public Text TPCollected;
    public SpriteRenderer OtherItem;
    public SpriteRenderer OtherBackground;
    public Text OtherCollected;

    // not owned and selected
    public SpriteRenderer NotOwnedSelectedAvatar;
    public GameObject StealBubble;

    // sprite inserts
    public Sprite emptyBasket;
    public List<Sprite> fullBaskets;
    public List<Sprite> avatars;
    public List<Sprite> otherItems;


    public void onStart()
    {
        // set the ownership of the basket
        belongsToPlayer = snhPlayerStats.playerID;

        // set the basket to empty
        Basket.sprite = emptyBasket;

        // set all the avatars
        UnselectedAvatar.sprite = avatars[snhPlayerStats.playerID];
        OwnedSelectedAvatar.sprite = avatars[snhPlayerStats.playerID];
        NotOwnedSelectedAvatar.sprite = avatars[snhPlayerStats.playerID];

        // default the states for owned selected
        TPCollected.text = formatString(snhPlayerStats.TPCollected);
        OtherItem.sprite = otherItems[gameConstants.OtherIndex];
        OtherCollected.text = formatString(snhPlayerStats.otherObjectCollected);

        // no one interacting with the basket
        engagedWithAnyPlayer = false;
        engagedWithPlayer = -1;

        // default to unselected
        Unselected.SetActive(true);
        OwnedSelected.SetActive(false);
        NotOwnedSelected.SetActive(false);
        StealBubble.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision!");
        if (collision.CompareTag("Player") && !engagedWithAnyPlayer)
        {
            engagedWithAnyPlayer = true;
            engagedWithPlayer = collision.GetComponent<SnHPlayerControlHandler>().playerID;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithAnyPlayer)
        {
            if (collision.GetComponent<SnHPlayerControlHandler>().playerID == engagedWithPlayer)
            {
                engagedWithAnyPlayer = false;
                engagedWithPlayer = -1;

                // switch back to unselected bubble
                Unselected.SetActive(true);
                OwnedSelected.SetActive(false);
                NotOwnedSelected.SetActive(false);
            }
        }
    }

    public void BelongsToPlayer()
    {
        // display correct bubble
        Unselected.SetActive(false);
        OwnedSelected.SetActive(true);
        NotOwnedSelected.SetActive(false);

    }

    public void DoesNotBelongToPlayer()
    {
        // display correct bubble
        Unselected.SetActive(false);
        OwnedSelected.SetActive(false);
        NotOwnedSelected.SetActive(true);
    }


    public void WrongPickupAdded()
    {
        // ADD ANIMATION
    }

    // confirm can be stolen from
    public SnHPickUps.PickUpType StolenFrom()
    {
        // when there are both TP and other to steal from
        if (snhPlayerStats.TPCollected != 0 && snhPlayerStats.otherObjectCollected != 0)
        {
            int selected = Random.Range(0, 1);
            if (selected == 0)
            {
                snhPlayerStats.TPCollected -= 1;
                Debug.Log("TP stolen from" + belongsToPlayer.ToString());
                return SnHPickUps.PickUpType.toiletPaper;
            }
            else
            {
                snhPlayerStats.otherObjectCollected -= 1;
                Debug.Log("Other object stolen from" + belongsToPlayer.ToString());
                return (SnHPickUps.PickUpType) gameConstants.OtherIndex;
            }
        }
        
        // only can steal toilet paper
        else if (snhPlayerStats.TPCollected != 0)
        {
            snhPlayerStats.TPCollected -= 1;
            Debug.Log("TP stolen from" + belongsToPlayer.ToString());
            return SnHPickUps.PickUpType.toiletPaper;
        }

        // only can steal other object
        else
        {
            snhPlayerStats.otherObjectCollected -= 1;
            Debug.Log("Other object stolen from" + belongsToPlayer.ToString());
            return (SnHPickUps.PickUpType) gameConstants.OtherIndex;
        }
    }

    // checks if the basket can be stolen from
    public void CanBeStolenFrom()
    {
        int totalItems = snhPlayerStats.TPCollected + snhPlayerStats.otherObjectCollected;
        if (totalItems > 0)
        {
            StealBubble.SetActive(true);
            canBeStolenFromBool = true;
        }
        else
        {
            StealBubble.SetActive(false);
            canBeStolenFromBool = false;
        }
    }

    public void Update()
    {
        // refresh the list for this basket
        TPCollected.text = formatString(snhPlayerStats.TPCollected);
        OtherCollected.text = formatString(snhPlayerStats.otherObjectCollected);

        // set the background colour to indicate if reached target numbers
        if (snhPlayerStats.TPCollected == gameConstants.CollectTP)
        {
            TPBackground.color = gameConstants.completeBackgroundColor;
        }
        else
        {
            TPBackground.color = gameConstants.incompleteBackgroundColor;
        }
        
        if (snhPlayerStats.otherObjectCollected == gameConstants.CollectOther)
        {
            OtherBackground.color = gameConstants.completeBackgroundColor;
        }
        else
        {
            OtherBackground.color = gameConstants.incompleteBackgroundColor;
        }
    }

    private string formatString(int score)
    {
        if (score.ToString().Length == 1)
        {
            return "0" + score.ToString();
        }
        else
        {
            return score.ToString();
        }
    }
}

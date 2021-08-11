using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// no manager
public class SnHBasketController : MonoBehaviour
{
    public Players players;

    public int playerID;

    private PlayerStats snhPlayerStats;

    public SnHGameConstants gameConstants;

    // is player engaged with basket now
    public int engagedWithPlayer;

    // can be stolen from
    public bool canBeStolenFromBool;

    // basket status
    public SpriteRenderer Basket;

    // to switch on and off when another player is nearby
    public GameObject Unselected;

    public GameObject OwnedSelected;

    public GameObject OwnedSelectedCorrect;

    public GameObject OwnedSelectedWrong;

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

    public Sprite fullBasket;

    public List<Sprite> itemSprites;

    private void Start()
    {
        engagedWithPlayer = -1;
        if (players.GetPlayers().ContainsKey(playerID))
        {
            snhPlayerStats = players.GetPlayers()[playerID].playerStats;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void onStart()
    {
        if (players.GetPlayers().ContainsKey(playerID))
        {
            snhPlayerStats = players.GetPlayers()[playerID].playerStats;

            // set the ownership of the basket
            // set the basket to empty
            Basket.sprite = emptyBasket;

            // set all the avatars
            UnselectedAvatar.sprite = snhPlayerStats.playerAvatar;
            OwnedSelectedAvatar.sprite = snhPlayerStats.playerAvatar;
            NotOwnedSelectedAvatar.sprite = snhPlayerStats.playerAvatar;

            // default the states for owned selected
            TPCollected.text = formatString(snhPlayerStats.TPCollected);
            OtherItem.sprite = itemSprites[(int) gameConstants.OtherIndex];
            OtherCollected.text =
                formatString(snhPlayerStats.otherObjectCollected);

            // no one interacting with the basket
            engagedWithPlayer = -1;

            // default to unselected
            Unselected.SetActive(true);
            OwnedSelected.SetActive(false);
            OwnedSelectedCorrect.SetActive(false);
            OwnedSelectedWrong.SetActive(false);
            NotOwnedSelected.SetActive(false);
            StealBubble.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer == -1)
        {
            engagedWithPlayer =
                collision
                    .GetComponent<PlayerStatsManager>()
                    .GetPlayerStats()
                    .playerID;

            if (engagedWithPlayer == playerID)
            {
                BelongsToPlayer();
            }
            else
            {
                DoesNotBelongToPlayer();
                CanBeStolenFrom();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer != -1)
        {
            if (
                collision
                    .GetComponent<PlayerStatsManager>()
                    .GetPlayerStats()
                    .playerID ==
                engagedWithPlayer
            )
            {
                engagedWithPlayer = -1;

                // switch back to unselected bubble
                Unselected.SetActive(true);
                OwnedSelected.SetActive(false);
                OwnedSelectedCorrect.SetActive(false);
                OwnedSelectedWrong.SetActive(false);
                NotOwnedSelected.SetActive(false);
                StealBubble.SetActive(false);
            }
        }
    }

    public void BelongsToPlayer()
    {
        // display correct bubble
        Unselected.SetActive(false);
        OwnedSelected.SetActive(true);
        NotOwnedSelected.SetActive(false);
        OwnedSelectedCorrect.SetActive(true);
        OwnedSelectedWrong.SetActive(false);
        StealBubble.SetActive(false);
    }

    public void DoesNotBelongToPlayer()
    {
        // display correct bubble
        Unselected.SetActive(false);
        OwnedSelected.SetActive(false);
        NotOwnedSelected.SetActive(true);
        OwnedSelectedCorrect.SetActive(false);
        OwnedSelectedWrong.SetActive(false);
        StealBubble.SetActive(false);
    }

    public void WrongPickupAdded()
    {
        OwnedSelectedCorrect.SetActive(false);
        OwnedSelectedWrong.SetActive(true);
    }

    public void IsPickedUp()
    {
        engagedWithPlayer = -1;
    }

    // confirm can be stolen from
    public PickUpTypeEnum StolenFrom()
    {
        // when there are both TP and other to steal from
        if (
            snhPlayerStats.TPCollected != 0 &&
            snhPlayerStats.otherObjectCollected != 0
        )
        {
            int selected = Random.Range(0, 1);
            if (selected == 0)
            {
                snhPlayerStats.TPCollected -= 1;
                Debug.Log("TP stolen from" + playerID.ToString());
                return PickUpTypeEnum.toiletPaper;
            }
            else
            {
                snhPlayerStats.otherObjectCollected -= 1;
                Debug.Log("Other object stolen from" + playerID.ToString());
                return (PickUpTypeEnum) gameConstants.OtherIndex;
            }
        } // only can steal toilet paper
        else if (snhPlayerStats.TPCollected != 0)
        {
            snhPlayerStats.TPCollected -= 1;
            Debug.Log("TP stolen from" + playerID.ToString());
            return PickUpTypeEnum.toiletPaper;
        }
        else
        // only can steal other object
        {
            snhPlayerStats.otherObjectCollected -= 1;
            Debug.Log("Other object stolen from" + playerID.ToString());
            return (PickUpTypeEnum) gameConstants.OtherIndex;
        }
    }

    // checks if the basket can be stolen from
    public void CanBeStolenFrom()
    {
        int totalItems =
            snhPlayerStats.TPCollected + snhPlayerStats.otherObjectCollected;
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

        // check if there are items in the basket
        int total_items =
            snhPlayerStats.TPCollected + snhPlayerStats.otherObjectCollected;
        if (total_items == 0)
        {
            Basket.sprite = emptyBasket;
        }
        else
        {
            Basket.sprite = fullBasket;
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

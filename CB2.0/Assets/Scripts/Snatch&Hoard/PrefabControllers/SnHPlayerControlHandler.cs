using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHPlayerControlHandler : MonoBehaviour
{
    public GameConstants constants;

    public Vector3GameEvent onDrop;

    public Vector3GameEvent onPickup;

    public GameEvent OnGameOver;

    public SnHGameConstants gameConstants;

    public PickUpTypeEnum heldPickUp;

    public Transform playerTransform;

    // pickup prefab
    public GameObject pickUpPrefab;

    // check if player is holding anything
    public bool isHoldingBasket;

    public bool isHoldingPickup;

    // basket reference
    public GameObject basketReference;

    // item bubble references
    public GameObject itemBubble;

    public SpriteRenderer itemSprite;

    // sprite list
    public List<Sprite> basketSprites; // 0 for empty, 1 for has items

    public List<Sprite> otherSprites; // hardcode

    // controllers from gameobjects in the zone
    public GameObject zoneObject = null;

    private PlayerStatsManager playerStatsManager;

    private PlayerController playerController;

    private int playerID;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerController = GetComponent<PlayerController>();
    }

    public void onMinigameStart()
    {
        itemBubble.SetActive(false);
        playerID = playerStatsManager.GetPlayerStats().playerID;
        isHoldingBasket = false;
        isHoldingPickup = false;
        heldPickUp = PickUpTypeEnum.noneType;
    }

    // reset player things when time ends
    public void onMinigameOver()
    {
        itemBubble.SetActive(false);
        playerController.EnableDash();
        playerController.RestoreMovement();
    }

    // entering zones
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // entered basket zone
        if (collision.CompareTag("Basket"))
        {
            // basket not engaged with anyone
            if (
                collision
                    .GetComponent<SnHBasketController>()
                    .engagedWithPlayer ==
                playerStatsManager.GetPlayerStats().playerID
            )
            {
                // implicitly not holding my basket
                if (
                    playerStatsManager.GetPlayerStats().playerID ==
                    collision
                        .GetComponent<SnHBasketController>()
                        .belongsToPlayer
                )
                {
                    playerStatsManager.GetPlayerStats().zoneType =
                        PlayerStats.ZoneType.myBasketZone;
                    zoneObject = collision.gameObject;
                }
                else // not my basket and i am not holding anything
                if (
                    playerStatsManager.GetPlayerStats().playerID !=
                    collision
                        .GetComponent<SnHBasketController>()
                        .belongsToPlayer &&
                    !isHoldingBasket &&
                    !isHoldingPickup
                )
                {
                    playerStatsManager.GetPlayerStats().zoneType =
                        PlayerStats.ZoneType.otherBasketZone;
                    zoneObject = collision.gameObject;
                }
            }
        }
        else // entered pickup zone
        if (collision.CompareTag("Pickup"))
        {
            // not holding anything and pickup is not engaged by anyone
            if (
                !isHoldingBasket &&
                !isHoldingPickup &&
                collision
                    .GetComponent<SnHPickUpController>()
                    .engagedWithPlayer ==
                playerStatsManager.GetPlayerStats().playerID
            )
            {
                playerStatsManager.GetPlayerStats().zoneType =
                    PlayerStats.ZoneType.pickUpZone;
                zoneObject = collision.gameObject;
            }
        }
        else // entered NPC zone
        if (collision.CompareTag("NPC"))
        {
            if (collision.GetComponent<SnHNPCController>().engagedWithPlayer == playerStatsManager.GetPlayerStats().playerID)
            {
                playerStatsManager.GetPlayerStats().zoneType = PlayerStats.ZoneType.NPCZone;
                zoneObject = collision.gameObject;
            }
        }

        // entered checkout zone
        else if (collision.CompareTag("Checkout"))
        {
            if (collision.GetComponent<SnHCheckoutController>().engagedWithPlayer == playerStatsManager.GetPlayerStats().playerID)
            {
                playerStatsManager.GetPlayerStats().zoneType = PlayerStats.ZoneType.CheckoutZone;
                zoneObject = collision.gameObject;
            }
        }
        // ADD MORE ZONES HANDLING
    }

    // exiting zones
    public void OnTriggerExit2D(Collider2D collision)
    {
        playerStatsManager.GetPlayerStats().zoneType =
            PlayerStats.ZoneType.NotInAnyZone;
        zoneObject = null;
    }

    // button press: use item
    // ADD INCOMPLETE CODE
    public void OnUsePower()
    {
        // ADD CODE
    }

    // button press: pick or drop items
    // ADD INCOMPLETE CODE
    public void OnPickDropItem()
    {
        // in my basket zone
        if (
            playerStatsManager.GetPlayerStats().zoneType ==
            PlayerStats.ZoneType.myBasketZone
        )
        {
            InMyBasketZone();
        }
        else // in other's basket zone
        if (
            playerStatsManager.GetPlayerStats().zoneType ==
            PlayerStats.ZoneType.otherBasketZone
        )
        {
            NotMyBasketZone();
        }
        else // in a pickup zone
        if (
            playerStatsManager.GetPlayerStats().zoneType ==
            PlayerStats.ZoneType.pickUpZone
        )
        {
            InPickUpZone();
        }
        else if (
            playerStatsManager.GetPlayerStats().zoneType ==
            PlayerStats.ZoneType.NPCZone
        )
        {
            InNPCZone();
        }
        else if (playerStatsManager.GetPlayerStats().zoneType == PlayerStats.ZoneType.CheckoutZone)
        {
            InCheckoutZone();
        }
        else // not in any zones but have things to do
        if (
            playerStatsManager.GetPlayerStats().zoneType ==
            PlayerStats.ZoneType.NotInAnyZone
        )
        {
            // if holding object basket, drop it
            if (isHoldingBasket)
            {
                isHoldingBasket = false;
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
                basketReference.transform.position = playerTransform.position;
                basketReference.SetActive(true);

                playerController.RestoreMovement();
                playerController.EnableDash();

                // remove reference to basket
                basketReference = null;
            }
            else // if holding object, drop it (spawn new object)
            if (isHoldingPickup)
            {
                Vector3 currentLocation =
                    new Vector3(playerTransform.position.x,
                        playerTransform.position.y,
                        playerTransform.position.z);
                GameObject newPickup =
                    Instantiate(pickUpPrefab,
                    currentLocation,
                    Quaternion.identity);
                newPickup
                    .GetComponent<SnHPickUpController>()
                    .SetPickUp(heldPickUp);

                onPickup.Fire (currentLocation);

                heldPickUp = PickUpTypeEnum.noneType;
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
                isHoldingPickup = false;

                playerController.EnableDash();
                playerController.RestoreMovement();
            }
        }
        // if in a particular zone, do actions according to the zone type
    }

    // button press: interacting with the shop
    public void OnShop()
    {
        // ADD CODE
    }

    private int _GetBasketStatus()
    {
        if (
            playerStatsManager.GetPlayerStats().otherObjectCollected != 0 ||
            playerStatsManager.GetPlayerStats().TPCollected != 0
        )
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // button pressed. drop pickup or pickup basket
    private void InMyBasketZone()
    {
        SnHBasketController bC = zoneObject.GetComponent<SnHBasketController>();

        // dropping pickup into basket
        if (isHoldingPickup)
        {
            // correct pickup
            if (
                heldPickUp == (PickUpTypeEnum) gameConstants.OtherIndex ||
                heldPickUp == PickUpTypeEnum.toiletPaper
            )
            {
                // add to the playerstats
                if (heldPickUp == PickUpTypeEnum.toiletPaper)
                {
                    playerStatsManager.GetPlayerStats().TPCollected += 1;
                }
                else
                {
                    playerStatsManager.GetPlayerStats().otherObjectCollected +=
                        1;
                }

                playerStatsManager.GetPlayerStats().score = computeScore();

                // no longer holding any pickups
                isHoldingPickup = false;
                heldPickUp = PickUpTypeEnum.noneType;
                playerController.EnableDash();
                playerController.RestoreMovement();

                // get rid of the item bubble
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
            }
            else
            // wrong pickup
            {
                bC.WrongPickupAdded();
            }
        }
        else
        // pickup basket
        {
            // make basket inactive in the hierarchy and store the reference here
            basketReference = zoneObject;
            bC.IsPickedUp();
            zoneObject.SetActive(false);
            isHoldingBasket = true;
            itemBubble.SetActive(true);
            itemSprite.sprite = basketSprites[_GetBasketStatus()];

            // compute player slowed multiplier
            playerController
                .SlowMovement(Mathf
                    .Pow(constants.SlowMovementFactor,
                    (
                    playerStatsManager.GetPlayerStats().TPCollected +
                    playerStatsManager.GetPlayerStats().otherObjectCollected
                    )));
            playerController.DisableDash();

            zoneObject = null;
        }
    }

    private int computeScore()
    {
        double percent = (double)(playerStatsManager.GetPlayerStats().TPCollected+playerStatsManager.GetPlayerStats().otherObjectCollected)/gameConstants.collectTotal;
        percent = Math.Truncate(percent * 100) / 100;
        return (int) percent * 100;
    }

    // button pressed. steal if possible
    private void NotMyBasketZone()
    {
        SnHBasketController oC = zoneObject.GetComponent<SnHBasketController>();

        // can steal
        if (oC.canBeStolenFromBool)
        {
            heldPickUp = oC.StolenFrom();

            // add to my stats
            if (heldPickUp == PickUpTypeEnum.toiletPaper)
            {
                playerStatsManager.GetPlayerStats().TPCollected += 1;
            }
            else
            {
                playerStatsManager.GetPlayerStats().otherObjectCollected += 1;
            }

            // display the item bubble
            itemSprite.sprite = otherSprites[(int) heldPickUp];
            itemBubble.SetActive(true);
        }
    }

    // button pressed. pickup item and destroy it in scene.
    private void InPickUpZone()
    {
        SnHPickUpController pC = zoneObject.GetComponent<SnHPickUpController>();

        // store information about the pickup
        heldPickUp = pC.pickupType;

        // display the item
        itemSprite.sprite = otherSprites[(int) heldPickUp];
        itemBubble.SetActive(true);

        // is holding item
        isHoldingPickup = true;

        // compute player slowed multiplier
        playerController.SlowMovement(constants.SlowMovementFactor);
        playerController.DisableDash();

        // remove spawn manager's reference to this pickup
        onDrop.Fire(zoneObject.transform.position);

        // destroy the game object itself
        Destroy(zoneObject.gameObject);

        // reset the reference to the game object back to null
        zoneObject = null;
    }

    // button pressed. give items to earn coins or get scolded for giving the wrong thing
    private void InNPCZone()
    {
        SnHNPCController npcC = zoneObject.GetComponent<SnHNPCController>();

        // not thinking and hence interactable
        if (!npcC.isThinking)
        {
            // gave correct item
            if (heldPickUp == npcC.expectedPickup)
            {
                npcC.CorrectPickupGiven();

                // get rid of what you are hold
                isHoldingPickup = false;
                heldPickUp = PickUpTypeEnum.noneType;
                playerController.EnableDash();
                playerController.RestoreMovement();

                // get rid of the item bubble
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
            }
            else
            {
                npcC.WrongPickupGiven();
            }
        }
    }

    private void InCheckoutZone()
    {
        SnHCheckoutController coC = zoneObject.GetComponent<SnHCheckoutController>();

        // check if i can checkout
        if (playerStatsManager.GetPlayerStats().TPCollected >= gameConstants.CollectTP && playerStatsManager.GetPlayerStats().otherObjectCollected >= gameConstants.CollectOther)
        {
            Debug.Log("checking out");

            // show the basket and finish the game
            coC.Checkout();

            // early game over
            OnGameOver.Fire();
        }
    }
}

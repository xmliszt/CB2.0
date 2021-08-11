using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnHPlayerControlHandler : MonoBehaviour
{
    public Vector3GameEvent onDrop;
    public Vector3GameEvent onPickup;

    public SnHPlayerStats snhPlayerStats;
    public SnHGameConstants gameConstants;
    public PickUpTypeEnum heldPickUp;

    public int playerID;
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


    public void onStart()
    {
        itemBubble.SetActive(false);
        playerID = snhPlayerStats.playerID;
        isHoldingBasket = false;
        isHoldingPickup = false;
        heldPickUp = PickUpTypeEnum.noneType;
    }

    // entering zones
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // entered basket zone
        if (collision.CompareTag("Basket"))
        {
            // basket not engaged with anyone
            if (collision.GetComponent<SnHBasketController>().engagedWithPlayer == snhPlayerStats.playerID)
            {
                // implicitly not holding my basket
                if (snhPlayerStats.playerID == collision.GetComponent<SnHBasketController>().belongsToPlayer)
                {
                    snhPlayerStats.zoneType = SnHPlayerStats.ZoneType.myBasketZone;
                    zoneObject = collision.gameObject;

                }

                // not my basket and i am not holding anything
                else if (snhPlayerStats.playerID != collision.GetComponent<SnHBasketController>().belongsToPlayer && !isHoldingBasket && !isHoldingPickup)
                {
                    snhPlayerStats.zoneType = SnHPlayerStats.ZoneType.otherBasketZone;
                    zoneObject = collision.gameObject;

                }
            }
        }
        
        // entered pickup zone
        else if (collision.CompareTag("Pickup"))
        {
            // not holding anything and pickup is not engaged by anyone
            if (!isHoldingBasket && !isHoldingPickup && collision.GetComponent<SnHPickUpController>().engagedWithPlayer == snhPlayerStats.playerID)
            {
                snhPlayerStats.zoneType = SnHPlayerStats.ZoneType.pickUpZone;
                zoneObject = collision.gameObject;
            }
        }
        
        // entered NPC zone
        else if (collision.CompareTag("NPC"))
        {
            snhPlayerStats.zoneType = SnHPlayerStats.ZoneType.NPCZone;
            zoneObject = collision.gameObject;
        }

        // ADD MORE ZONES HANDLING


    }

    // exiting zones
    public void OnTriggerExit2D(Collider2D collision)
    {
        snhPlayerStats.zoneType = SnHPlayerStats.ZoneType.NotInAnyZone;
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
        if(snhPlayerStats.zoneType == SnHPlayerStats.ZoneType.myBasketZone)
        {
            InMyBasketZone();
        }
        
        // in other's basket zone
        else if (snhPlayerStats.zoneType == SnHPlayerStats.ZoneType.otherBasketZone)
        {
            NotMyBasketZone();
        }

        // in a pickup zone
        else if (snhPlayerStats.zoneType == SnHPlayerStats.ZoneType.pickUpZone)
        {
            InPickUpZone();
        }

        else if (snhPlayerStats.zoneType == SnHPlayerStats.ZoneType.NPCZone)
        {
            InNPCZone();
        }

        // not in any zones but have things to do
        else if (snhPlayerStats.zoneType == SnHPlayerStats.ZoneType.NotInAnyZone)
        {
            // if holding object basket, drop it
            if (isHoldingBasket)
            {
                isHoldingBasket = false;
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
                basketReference.transform.position = playerTransform.position;
                basketReference.SetActive(true);

                // compute player slowed multiplier
                snhPlayerStats.PlayerSpeed = 1;

                // remove reference to basket
                basketReference = null;
            }

            // if holding object, drop it (spawn new object)
            else if (isHoldingPickup)
            {
                Vector3 currentLocation = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
                GameObject newPickup = Instantiate(pickUpPrefab, currentLocation, Quaternion.identity);
                newPickup.GetComponent<SnHPickUpController>().SetPickUp(heldPickUp);

                onPickup.Fire(currentLocation);

                heldPickUp = PickUpTypeEnum.noneType;
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
                isHoldingPickup = false;
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
        if (snhPlayerStats.otherObjectCollected != 0 || snhPlayerStats.TPCollected != 0)
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
            if (heldPickUp == (PickUpTypeEnum)gameConstants.OtherIndex || heldPickUp == PickUpTypeEnum.toiletPaper)
            {
                // add to the playerstats
                if (heldPickUp == PickUpTypeEnum.toiletPaper)
                {
                    snhPlayerStats.TPCollected += 1;
                }
                else
                {
                    snhPlayerStats.otherObjectCollected += 1;
                }

                // no longer holding any pickups
                isHoldingPickup = false;
                heldPickUp = PickUpTypeEnum.noneType;

                // get rid of the item bubble
                itemSprite.sprite = null;
                itemBubble.SetActive(false);
            }

            // wrong pickup
            else
            {
                bC.WrongPickupAdded();
            }
        }

        // pickup basket
        else
        {
            // make basket inactive in the hierarchy and store the reference here
            basketReference = zoneObject;
            bC.IsPickedUp();
            zoneObject.SetActive(false);
            isHoldingBasket = true;
            itemBubble.SetActive(true);
            itemSprite.sprite = basketSprites[_GetBasketStatus()];

            // compute player slowed multiplier
            snhPlayerStats.PlayerSpeed = Mathf.Pow(snhPlayerStats.SlowMultiplier, (snhPlayerStats.TPCollected + snhPlayerStats.otherObjectCollected));

            zoneObject = null;
        }
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
                snhPlayerStats.TPCollected += 1;
            }
            else
            {
                snhPlayerStats.otherObjectCollected += 1;
            }

            // display the item bubble
            itemSprite.sprite = otherSprites[(int)heldPickUp];
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
        itemSprite.sprite = otherSprites[(int)heldPickUp];
        itemBubble.SetActive(true);

        // is holding item
        isHoldingPickup = true;

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
}

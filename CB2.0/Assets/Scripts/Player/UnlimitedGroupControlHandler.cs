using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupControlHandler : MonoBehaviour
{
    public GameConstants constants;

    [Header("Item Types")]

    public Item[] shopItemList;

    [Header("Grab Attributes")]
    public Transform grabDetect;
    public bool held = false;

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;
    public GameObject rechargeBarObject;

    [Header("Physical Item Prefab")]
    public GameObject swabStickPrefab;

    private ShopHandler shopHandler;

    private GameObject pickedItem; // the item player picked up

    private PlayerInventory inventory;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerController playerController;

    private int layerMask;

    private EntertainmentController entertainmentController;

    private bool available = true;

    private Item shopItem;

    private RechargeBar rechargeBar;



    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();

        rechargeBar = rechargeBarObject.GetComponent<RechargeBar>();

        layerMask = LayerMask.GetMask("Entertainments");

    }

    private void Start()
    {
        thoughtBubbleRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
        inventory = playerStatsManager.GetPlayerStats().inventory;

        // To Change
        playerStatsManager.GetPlayerStats().score = 0;
        playerStatsManager.GetPlayerStats().coins = 50;
    }

    private void Update()
    {
        
        RaycastHit2D grabCheck = 
        Physics2D.CircleCast(grabDetect.position, constants.castRadius, Vector2.down * transform.localScale, constants.castRadius, layerMask);

        if(grabCheck.collider != null && grabCheck.collider.tag == "Entertainments")
        {
            
            if (available) {
                entertainmentController = grabCheck.collider.gameObject.GetComponent<EntertainmentController>();
                entertainmentController.fromPlayer = gameObject;
                entertainmentController.SetSpriteOutline();
                available = false;
            }
            
            // Can check if entertainment controller is "held" too
            if (held && entertainmentController) {
                // Slow down player & Disable dash
                playerController.SlowMovement(constants.slowFactor);
                playerController.DisableDash();

                // Move entertainment object
                entertainmentController.MoveItem();

            }
            
        }
        // Object out of range: Deselect 
        else {
            if (entertainmentController != null) {
                entertainmentController.DisableSpriteOutline();
                entertainmentController = null;
                available = true;
            }
        }

        // Edge case where player is detecting another entertainment: Deselect
        if (grabCheck.collider != null && 
        grabCheck.collider.tag == "Entertainments" && 
        entertainmentController != grabCheck.collider.gameObject.GetComponent<EntertainmentController>()) {
            entertainmentController.DisableSpriteOutline();
            entertainmentController = null;
            available = true;
        }

        if (!held) {
            // Player resumes normal speed and dash
            playerController.EnableMovement();
            playerController.EnableDash();

        }
    }

    public void OnUse()
    {
        if (inventory.hasItem())
        {
            Item currentItem = inventory.GetCurrentItem();
            Vector2 idleDirection = playerController.GetIdleDirection();
            switch(currentItem.itemType)
            {
                case Item.ItemType.shopItem:
                    if (entertainmentController) {
                        
                        
                        // Check the item type
                        if (currentItem.itemName == "lock" && !entertainmentController.locked)
                        {
                            // Check if entertainment already has existing lock
                            entertainmentController.SetLock();
                            inventory.useItem();
                            thoughtBubbleRenderer.enabled = false;
                            Debug.Log("USED LOCK");
                        }
                        else if (currentItem.itemName == "upgrade" && !entertainmentController.upgraded)
                        {
                            // Check if entertainment already has the existing upgrade
                            entertainmentController.SetUpgrade();
                            inventory.useItem();
                            thoughtBubbleRenderer.enabled = false;
                            Debug.Log("USED UPGRADE");
                        }
                        
                    }
                    break;

                default:
                    break;
            }
        }

    }

    public void OnPickUpDrop()
    {
        shootSwabStick();
    }


    public void OnShop()
    {
        if (
            !inventory.hasItem() &&
            playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.shop
        )
        {
            Debug.Log("IM USING SHOP");
            ShopItem boughtItem = shopHandler.BuyItem(gameObject);

            switch(boughtItem.itemName)
            {
                case "lock":
                    shopItem = shopItemList[0];
                    break;
                case "upgrade":
                    shopItem = shopItemList[1];
                    break;
                default:
                    break;
            }

            if (boughtItem != null)
            {
                inventory.SetItem (shopItem);

                Debug.Log("ITEM BOUGHT");

                thoughtBubbleRenderer.sprite = shopItem.thoughtBubbleSprite;
                thoughtBubbleRenderer.enabled = true;

                Debug.Log(playerStatsManager.GetPlayerStats().coins);
                Item currentItem = inventory.GetCurrentItem();
                Debug.Log(currentItem.itemSprite.name);
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {   
        held = context.ReadValueAsButton();
    }

    public void SetPickedItem(GameObject _pickedItem)
    {
        pickedItem = _pickedItem;
    }

    public void SetShopHandler(ShopHandler _shopHandler)
    {
        shopHandler = _shopHandler;
    }

    // public void UpdateScore(int score) {
    //     playerStatsManager.GetPlayerStats().score += score;
    //     Debug.Log(playerStatsManager.GetPlayerStats().score);
    // }

    
    // Shooting mechanism
    
    private void shootSwabStick() {

        if (rechargeBar.GetRecharge() >= constants.shootEnergy)
        {
            rechargeBar.UseRecharge(constants.shootEnergy);
            Vector2 idleDirection = playerController.GetIdleDirection();
            
            GameObject stick =
                Instantiate(swabStickPrefab,
                transform.position +
                new Vector3(idleDirection.x,
                    idleDirection.y - 0.2f,
                    transform.position.z) *
                0.5f,
                swabStickPrefab.transform.rotation);
            StickMovement stickMovementScript =
                stick.GetComponent<StickMovement>();
            stickMovementScript.fromPlayer = gameObject;
            stickMovementScript.direction = idleDirection;
            stickMovementScript.StartFlying();
        }

    }

    public void OnStickHit()
    {
        playerStatsManager.GetPlayerStats().coins += 1; // Change to game constants
        Debug.Log(playerStatsManager.GetPlayerStats().coins);
    }

    public void GetStickHit()
    {
        // TODO: Reset position to start
        Debug.Log("I'M HIT");
    }




}

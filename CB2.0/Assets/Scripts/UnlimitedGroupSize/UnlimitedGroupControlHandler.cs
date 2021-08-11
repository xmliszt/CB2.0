using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupControlHandler : MonoBehaviour
{
    public GameConstants constants;
    public PlayerRelocateGameEvent playerRelocateGameEvent;

    [Header("Item Types")]
    public Item[] shopItemList;

    [Header("Grab Attributes")]
    public GameObject grabDetect;
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
    private PlayerAudioController playerAudioController;

    private int layerMask;
    private EntertainmentController entertainmentController;
    private bool available = true;
    private Item shopItem;
    private RechargeBar rechargeBar;
    private bool isGameStarted = false;
    private float initialPlayerSpeed;

    private void Awake()
    {
        playerAudioController = GetComponent<PlayerAudioController>();
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();

        rechargeBar = rechargeBarObject.GetComponent<RechargeBar>();
        layerMask = LayerMask.GetMask("Entertainments");
        initialPlayerSpeed = constants.playerMoveSpeed;
    }

    private void Start()
    {
        thoughtBubbleRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
        inventory = playerStatsManager.GetPlayerStats().inventory;
    }

    private void Update()
    {
        if (isGameStarted)
        {
            RaycastHit2D grabCheck =
                Physics2D
                    .CircleCast(grabDetect.transform.position,
                    constants.castRadius,
                    Vector2.down * transform.localScale,
                    constants.castRadius,
                    layerMask);

            if (
                grabCheck.collider != null &&
                grabCheck.collider.tag == "Entertainments"
            )
            {
                if (available)
                {
                    entertainmentController =
                        grabCheck
                            .collider
                            .gameObject
                            .GetComponent<EntertainmentController>();
                    if (!entertainmentController.fromPlayer)
                    {
                        entertainmentController.fromPlayer = gameObject;
                        entertainmentController.SetSpriteOutline();
                        available = false;
                    }
                }

                // Can check if entertainment controller is "held" too
                if (
                    held &&
                    entertainmentController &&
                    entertainmentController.fromPlayer == gameObject
                )
                {
                    
                    // Slow down player & Disable dash
                    playerController.SlowMovement(constants.slowFactor);
                    playerController.DisableDash();

                    // Move entertainment object
                    entertainmentController.MoveItem();
                }
            }
            else
            // Object out of range: Deselect
            {
                if (
                    entertainmentController != null &&
                    entertainmentController.fromPlayer == gameObject
                )
                {
                    deselectEntertainment();
                }
            }

            // Edge case where player is detecting another entertainment: Deselect
            if (
                grabCheck.collider != null &&
                grabCheck.collider.tag == "Entertainments" &&
                entertainmentController !=
                grabCheck
                    .collider
                    .gameObject
                    .GetComponent<EntertainmentController>() &&
                entertainmentController.fromPlayer == gameObject
            )
            {
                deselectEntertainment();
            }

            if (!held 
            && entertainmentController 
            && entertainmentController.fromPlayer == gameObject)
            {
                // Player resumes normal speed and dash
                playerController.RestoreMovement();
                playerController.EnableDash();
            }
        }
    }

    private void deselectEntertainment()
    {
        entertainmentController.DisableSpriteOutline();
        entertainmentController.fromPlayer = null;
        entertainmentController = null;
        available = true;
    }

    public void OnUse()
    {
        if (inventory.hasItem())
        {
            Item currentItem = inventory.GetCurrentItem();
            Vector2 idleDirection = playerController.GetIdleDirection();
            switch (currentItem.itemType)
            {
                case Item.ItemType.shopItem:
                    if (entertainmentController)
                    {
                        // Check the item type
                        // Check if entertainment already has existing lock
                        if (
                            currentItem.itemName == "lock" &&
                            !entertainmentController.locked
                        )
                        {
                            playerAudioController.PlaySFX(SFXType._lock);
                            entertainmentController.SetLock();
                            inventory.useItem();
                            thoughtBubbleRenderer.enabled = false;
                        }
                        // Check if entertainment already has the existing upgrade
                        else if (
                            currentItem.itemName == "upgrade" &&
                            !entertainmentController.upgraded
                        )
                        {
                            playerAudioController.PlaySFX(SFXType._lock);
                            entertainmentController.SetUpgrade();
                            inventory.useItem();
                            thoughtBubbleRenderer.enabled = false;
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
            playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.ugsShop
        )
        {
            ShopItem boughtItem = shopHandler.BuyItem(gameObject);

            switch (boughtItem.itemName)
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
                playerAudioController.PlaySFX(SFXType._lock);
                inventory.SetItem (shopItem);

                thoughtBubbleRenderer.sprite = shopItem.thoughtBubbleSprite;
                thoughtBubbleRenderer.enabled = true;

                Debug.Log(playerStatsManager.GetPlayerStats().coins);
                Item currentItem = inventory.GetCurrentItem();
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if (entertainmentController)
        {
            playerAudioController.PlaySFX(SFXType.drop);
        }
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

    // Shooting mechanism
    private void shootSwabStick()
    {
        if (rechargeBar.GetRecharge() >= constants.shootEnergy)
        {
            playerAudioController.PlaySFX(SFXType.shoot);
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
        playerAudioController.PlaySFX(SFXType.coin);
        playerStatsManager.GetPlayerStats().coins += constants.onHitRewardCoins;
    }

    public void GetStickHit()
    {
        playerController.RestoreMovement();
        playerController.EnableDash();
        int playerID = playerStatsManager.GetPlayerStats().playerID;
        
        // To detach entertainment object from player when attacked
        deselectEntertainment();
        held = false;
        entertainmentController = null;
    
        playerRelocateGameEvent.Fire(playerID, FindObjectOfType<UnlimitedGroupManager>().GetPlayerLocation(playerID));
    }

    public void onMinigameStart()
    {
        rechargeBarObject.SetActive(true);
        grabDetect.SetActive(true);
        isGameStarted = true;
    }

    // Reset all minigame-specific player appearance
    public void onMinigameOver()
    {
        inventory.ClearItem();
        thoughtBubbleRenderer.enabled = false;
        rechargeBarObject.SetActive(false);
        grabDetect.SetActive(false);
        isGameStarted = false;
        playerController.RestoreMovement();
        playerController.EnableDash();
    }
}

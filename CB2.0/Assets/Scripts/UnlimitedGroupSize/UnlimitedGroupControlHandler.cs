using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupControlHandler : MonoBehaviour
{
    public GameStats gameStats;

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

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerController playerController;

    private PlayerAudioController playerAudioController;

    private ControlKeyIndicatorHandler controlKeyIndicatorHandler;

    private int layerMask;

    private EntertainmentController entertainmentController;

    private bool available = true;

    private bool entertainmentChanged = false;

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
        controlKeyIndicatorHandler = GetComponent<ControlKeyIndicatorHandler>();

        rechargeBar = rechargeBarObject.GetComponent<RechargeBar>();
        layerMask = LayerMask.GetMask("Entertainments");
        initialPlayerSpeed = constants.playerMoveSpeed;
    }

    private void Start()
    {
        thoughtBubbleRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
        if (gameStats.tutorialModeOn)
        {
            controlKeyIndicatorHandler.TurnOnIndicator(ControllerKeyType.south);
        }
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
                        if (gameStats.tutorialModeOn)
                        {
                            controlKeyIndicatorHandler
                                .TurnOnIndicator(ControllerKeyType.east);
                        }
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
                else if (
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

            if (!held)
            {
                // Player resumes normal speed and dash
                playerController.RestoreMovement();
                playerController.EnableDash();

                // Edge case: Entertainment controller does not belong to you
                if (entertainmentController != null && entertainmentController.fromPlayer != gameObject)
                {
                    deselectEntertainment();
                }
            }
            if (playerStatsManager.GetPlayerStats().item != null)
            {
                if (
                    gameStats.tutorialModeOn &&
                    entertainmentController &&
                    playerStatsManager.GetPlayerStats().item.itemName ==
                    "lock" &&
                    !entertainmentController.locked
                )
                {
                    controlKeyIndicatorHandler
                        .TurnOnIndicator(ControllerKeyType.west);
                }
                else if (
                    gameStats.tutorialModeOn &&
                    entertainmentController &&
                    playerStatsManager.GetPlayerStats().item.itemName ==
                    "upgrade" &&
                    !entertainmentController.upgraded
                )
                {
                    controlKeyIndicatorHandler
                        .TurnOnIndicator(ControllerKeyType.west);
                }
            }
            else
            {
                if (
                    gameStats.tutorialModeOn &&
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.ugsShop
                )
                {
                    controlKeyIndicatorHandler
                        .TurnOnIndicator(ControllerKeyType.north);
                }
                else if (
                    rechargeBar.GetRecharge() >= constants.shootEnergy &&
                    playerStatsManager.GetPlayerStats() &&
                    !held &&
                    gameStats.tutorialModeOn
                )
                {
                    controlKeyIndicatorHandler
                        .TurnOnIndicator(ControllerKeyType.south);
                }
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
        if (playerStatsManager.GetPlayerStats().item)
        {
            Item currentItem = playerStatsManager.GetPlayerStats().item;
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
                            playerStatsManager.GetPlayerStats().item = null;
                            thoughtBubbleRenderer.enabled = false;
                        } // Check if entertainment already has the existing upgrade
                        else if (
                            currentItem.itemName == "upgrade" &&
                            !entertainmentController.upgraded
                        )
                        {
                            playerAudioController.PlaySFX(SFXType._lock);
                            entertainmentController.SetUpgrade();
                            playerStatsManager.GetPlayerStats().item = null;
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
            !playerStatsManager.GetPlayerStats().item &&
            playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.ugsShop
        )
        {
            ShopItem boughtItem = shopHandler.BuyItem(gameObject);

            if (boughtItem != null)
            {
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
                playerAudioController.PlaySFX(SFXType._lock);
                playerStatsManager.GetPlayerStats().item = shopItem;

                thoughtBubbleRenderer.sprite = shopItem.thoughtBubbleSprite;
                thoughtBubbleRenderer.enabled = true;

                Item currentItem = playerStatsManager.GetPlayerStats().item;
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
            if (gameStats.tutorialModeOn)
            {
                controlKeyIndicatorHandler.TurnOffIndiciator();
            }
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

        // Edge case: To detach entertainment object from player when attacked
        if (entertainmentController) deselectEntertainment();
        held = false;
        entertainmentController = null;

        playerRelocateGameEvent
            .Fire(playerID,
            FindObjectOfType<UnlimitedGroupManager>()
                .GetPlayerLocation(playerID));
    }

    public void onMinigameStart()
    {
        if (gameStats.GetCurrentScene() == GameStats.Scene.unlimitedGroupSize)
        {
            rechargeBarObject.SetActive(true);
            grabDetect.SetActive(true);
            isGameStarted = true;
        }
    }

    // Reset all minigame-specific player appearance
    public void onMinigameOver()
    {
        if (gameStats.GetCurrentScene() == GameStats.Scene.unlimitedGroupSize)
        {
            playerStatsManager.GetPlayerStats().item = null;
            thoughtBubbleRenderer.enabled = false;
            rechargeBarObject.SetActive(false);
            grabDetect.SetActive(false);
            isGameStarted = false;
            playerController.RestoreMovement();
            playerController.EnableDash();
        }
    }
}

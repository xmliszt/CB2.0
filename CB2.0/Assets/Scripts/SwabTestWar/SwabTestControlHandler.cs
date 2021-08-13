using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwabTestControlHandler : MonoBehaviour
{
    public GameConstants constants;

    public GameStats gameStats;

    [Header("Item Types")]
    public Item swabStick;

    public Item testSample;

    public Item testResult;

    public Item trash;

    public Item shopItem;

    [Header("Physical Item Prefab")]
    public GameObject swabStickPrefab;

    public GameObject droppedItemPrefab;

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;

    public SpriteRenderer stunnedIconRenderer;

    private TestSampleProcessor testStationProcessor; // the test station where the player is at

    private ShopHandler shopHandler;

    private GameObject pickedItem; // the item player picked up

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private ControlKeyIndicatorHandler controlKeyIndicatorHandler;

    private PlayerController playerController;

    private PlayerAudioController playerAudioController;

    private bool autoPickEnabled = true;

    private bool isGameEnded = true;

    private void Awake()
    {
        playerAudioController = GetComponent<PlayerAudioController>();
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
        controlKeyIndicatorHandler = GetComponent<ControlKeyIndicatorHandler>();
    }

    private void Start()
    {
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
    }

    private void Update()
    {
        if (!isGameEnded)
        {
            // auto pick
            if (
                playerZoneManager.GetZone() ==
                PlayerZoneManager.ZoneType.droppedItem &&
                !playerStatsManager.GetPlayerStats().item &&
                autoPickEnabled
            )
            {
                // pick up dropped item
                if (pickedItem)
                {
                    Item _item =
                        pickedItem.GetComponent<CollectableItem>().itemMeta;
                    playerStatsManager.GetPlayerStats().item = _item;
                    thoughtBubbleRenderer.sprite = _item.thoughtBubbleSprite;
                    thoughtBubbleRenderer.enabled = true;
                    Destroy (pickedItem);
                }
            }

            // tutorial mode
            if (gameStats.tutorialModeOn)
            {
                switch (playerZoneManager.GetZone())
                {
                    case PlayerZoneManager.ZoneType.swabStickCollection:
                        if (playerStatsManager.GetPlayerStats().item == null)
                        {
                            controlKeyIndicatorHandler
                                .TurnOnIndicator(ControllerKeyType.south);
                        }
                        break;
                    case PlayerZoneManager.ZoneType.testStation:
                        if (playerStatsManager.GetPlayerStats().item == null)
                        {
                            if (testStationProcessor.testStationInfo.isComplete)
                            {
                                controlKeyIndicatorHandler
                                    .TurnOnIndicator(ControllerKeyType.south);
                            }
                        }
                        else
                        {
                            if (
                                playerStatsManager
                                    .GetPlayerStats()
                                    .item
                                    .itemType ==
                                Item.ItemType.testSample ||
                                playerStatsManager
                                    .GetPlayerStats()
                                    .item
                                    .itemType ==
                                Item.ItemType.shopItem
                            )
                            {
                                controlKeyIndicatorHandler
                                    .TurnOnIndicator(ControllerKeyType.west);
                            }
                        }
                        break;
                    case PlayerZoneManager.ZoneType.submissionStation:
                        if (
                            playerStatsManager.GetPlayerStats().item.itemType ==
                            Item.ItemType.testResult
                        )
                        {
                            controlKeyIndicatorHandler
                                .TurnOnIndicator(ControllerKeyType.west);
                        }
                        break;
                    case PlayerZoneManager.ZoneType.dustbin:
                        if (playerStatsManager.GetPlayerStats().item != null)
                        {
                            if (
                                playerStatsManager
                                    .GetPlayerStats()
                                    .item
                                    .itemType ==
                                Item.ItemType.trash
                            )
                            {
                                controlKeyIndicatorHandler
                                    .TurnOnIndicator(ControllerKeyType.west);
                            }
                        }
                        break;
                    case PlayerZoneManager.ZoneType.shop:
                        if (playerStatsManager.GetPlayerStats().item == null)
                        {
                            controlKeyIndicatorHandler
                                .TurnOnIndicator(ControllerKeyType.north);
                        }
                        break;
                    default:
                        if (playerStatsManager.GetPlayerStats().item != null)
                        {
                            if (
                                playerStatsManager
                                    .GetPlayerStats()
                                    .item
                                    .itemType ==
                                Item.ItemType.swabStick
                            )
                            {
                                controlKeyIndicatorHandler
                                    .TurnOnIndicator(ControllerKeyType.west);
                            }
                        }
                        else
                        {
                            Debug.Log("swab test turn off indicator");
                            controlKeyIndicatorHandler.TurnOffIndiciator();
                        }
                        break;
                }
            }
        }
    }

    public void OnUse()
    {
        if (playerStatsManager.GetPlayerStats().item != null)
        {
            Item currentItem = playerStatsManager.GetPlayerStats().item;
            Vector2 idleDirection = playerController.GetIdleDirection();
            if (currentItem.itemType == Item.ItemType.swabStick)
            {
                playerAudioController.PlaySFX(SFXType.shoot);
                playerStatsManager.GetPlayerStats().item = null;
                GameObject stick =
                    Instantiate(swabStickPrefab,
                    transform.position +
                    new Vector3(idleDirection.x,
                        idleDirection.y - 0.2f,
                        transform.position.z) *
                    0.5f,
                    swabStickPrefab.transform.rotation);
                SwabStickMovement stickMovementScript =
                    stick.GetComponent<SwabStickMovement>();
                stickMovementScript.fromPlayer = gameObject;
                stickMovementScript.direction = idleDirection;
                stickMovementScript.StartFlying();
                thoughtBubbleRenderer.enabled = false;
            }
            else if (currentItem.itemType == Item.ItemType.testSample)
            {
                // submit test sample to test station
                if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.testStation
                )
                {
                    if (!testStationProcessor.testStationInfo.isLoaded)
                    {
                        if (testStationProcessor.testStationInfo.isLocked)
                        {
                            if (
                                transform.GetInstanceID() ==
                                testStationProcessor.testStationInfo.resultOwner
                            )
                            {
                                SubmitTestSample();
                            }
                        }
                        else
                        {
                            SubmitTestSample();
                        }
                    }
                }
            }
            else if (currentItem.itemType == Item.ItemType.testResult)
            {
                // submit test result to submission desk
                if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.submissionStation
                )
                {
                    playerAudioController.PlaySFX(SFXType.submitResult);
                    playerStatsManager.GetPlayerStats().item = trash;
                    thoughtBubbleRenderer.sprite = trash.thoughtBubbleSprite;
                    thoughtBubbleRenderer.enabled = true;

                    // Log 1 completed swab test
                    playerStatsManager.GetPlayerStats().score++;
                }
            }
            else if (currentItem.itemType == Item.ItemType.trash)
            {
                // throw the trash and gain coins
                if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.dustbin
                )
                {
                    playerStatsManager.GetPlayerStats().item = null;
                    thoughtBubbleRenderer.enabled = false;

                    // Gain 1 coin!
                    playerAudioController.PlaySFX(SFXType.coin);
                    playerStatsManager.GetPlayerStats().coins +=
                        constants.coinAwardedPerCompleteTest;
                }
            }
            else if (currentItem.itemType == Item.ItemType.shopItem)
            {
                if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.testStation
                )
                {
                    playerAudioController.PlaySFX(SFXType._lock);
                    playerStatsManager.GetPlayerStats().item = null;
                    thoughtBubbleRenderer.enabled = false;
                    testStationProcessor.OnLock (gameObject);
                }
            }
        }
    }

    public void onPickUpDrop()
    {
        if (!playerStatsManager.GetPlayerStats().item)
        {
            if (
                playerZoneManager.GetZone() !=
                PlayerZoneManager.ZoneType.nullType
            )
            {
                playerAudioController.PlaySFX(SFXType.drop); // pick up same sound
                if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.swabStickCollection
                )
                {
                    playerStatsManager.GetPlayerStats().item = swabStick;
                    thoughtBubbleRenderer.sprite =
                        swabStick.thoughtBubbleSprite;
                    thoughtBubbleRenderer.enabled = true;
                }
                else if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.testStation
                )
                {
                    if (testStationProcessor.testStationInfo.isComplete)
                    {
                        if (testStationProcessor.testStationInfo.isLocked)
                        {
                            if (
                                transform.GetInstanceID() ==
                                testStationProcessor.testStationInfo.resultOwner
                            )
                            {
                                PickUpTestResult();
                            }
                        }
                        else
                        {
                            PickUpTestResult();
                        }
                    }
                }
            }
        }
        else
        {
            playerAudioController.PlaySFX(SFXType.drop);
            DropItem();

            // disable auto pick up for a short while
            autoPickEnabled = false;
            StartCoroutine(EnableAutoPickUp(1));
        }
    }

    IEnumerator EnableAutoPickUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        autoPickEnabled = true;
    }

    public void onShop()
    {
        if (
            !playerStatsManager.GetPlayerStats().item &&
            playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.shop
        )
        {
            ShopItem boughtItem = shopHandler.BuyItem(gameObject);
            if (boughtItem != null)
            {
                playerStatsManager.GetPlayerStats().item = shopItem;
                thoughtBubbleRenderer.sprite = shopItem.thoughtBubbleSprite;
                thoughtBubbleRenderer.enabled = true;
            }
        }
    }

    private void SubmitTestSample()
    {
        playerAudioController.PlaySFX(SFXType.drop);
        testStationProcessor.OnLoadTestSample(transform.GetInstanceID());
        playerStatsManager.GetPlayerStats().item = null;
        thoughtBubbleRenderer.enabled = false;
    }

    private void DropItem()
    {
        // drop item
        if (playerStatsManager.GetPlayerStats().item)
        {
            Item _item = playerStatsManager.GetPlayerStats().item;
            playerStatsManager.GetPlayerStats().item = null;
            Vector2 idleDirection = playerController.GetIdleDirection();
            GameObject dropped =
                Instantiate(droppedItemPrefab,
                transform.position +
                new Vector3(idleDirection.x,
                    idleDirection.y,
                    droppedItemPrefab.transform.position.z) *
                0.2f,
                droppedItemPrefab.transform.rotation);
            dropped.GetComponent<CollectableItem>().SetItem(_item);
            thoughtBubbleRenderer.enabled = false;
        }
    }

    private void PickUpTestResult()
    {
        testStationProcessor.OnRetrieveTestResult(transform.GetInstanceID());
        playerStatsManager.GetPlayerStats().item = testResult;
        thoughtBubbleRenderer.sprite = testResult.thoughtBubbleSprite;
        thoughtBubbleRenderer.enabled = true;
    }

    public void GetStunned()
    {
        DropItem();
        autoPickEnabled = false;
        StartCoroutine(EnableAutoPickUp(constants.playerStunnedDuration));
        stunnedIconRenderer.enabled = true;
        SpriteRenderer _renderer = GetComponent<SpriteRenderer>();
        playerController.DisableMovement();
        playerController.DisableController();
        _renderer.color =
            new Color(_renderer.color.r,
                _renderer.color.g,
                _renderer.color.b,
                0.7f);
        StartCoroutine(Unfreeze());
    }

    IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(constants.playerStunnedDuration);
        playerController.EnableController();
        playerController.EnableMovement();
        stunnedIconRenderer.enabled = false;
        SpriteRenderer _renderer = GetComponent<SpriteRenderer>();
        _renderer.color =
            new Color(_renderer.color.r,
                _renderer.color.g,
                _renderer.color.b,
                1);
    }

    public void OnSwabStickHit()
    {
        playerAudioController.PlaySFX(SFXType.hit);
        playerStatsManager.GetPlayerStats().item = testSample;
        thoughtBubbleRenderer.sprite = testSample.thoughtBubbleSprite;
        thoughtBubbleRenderer.enabled = true;
    }

    public void SetTestStationProcessor(
        TestSampleProcessor _testSampleProcessor
    )
    {
        testStationProcessor = _testSampleProcessor;
    }

    public void SetPickedItem(GameObject _pickedItem)
    {
        pickedItem = _pickedItem;
    }

    public void SetShopHandler(ShopHandler _shopHandler)
    {
        shopHandler = _shopHandler;
    }

    public void onMinigameStart()
    {
        if (gameStats.GetCurrentScene() == GameStats.Scene.swabTestWar)
            isGameEnded = false;
    }

    // Reset all minigame-specific player appearance
    public void onMinigameOver()
    {
        if (gameStats.GetCurrentScene() == GameStats.Scene.swabTestWar)
        {
            thoughtBubbleRenderer.enabled = false;
            stunnedIconRenderer.enabled = false;
            isGameEnded = true;
        }
    }
}

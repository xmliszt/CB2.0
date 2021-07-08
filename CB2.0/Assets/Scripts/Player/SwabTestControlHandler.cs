using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwabTestControlHandler : MonoBehaviour
{
    public GameConstants constants;

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

    private PlayerInventory inventory;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerController playerController;

    private bool autoPickEnabled = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
    }

    private void Start()
    {
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
        inventory = playerStatsManager.GetPlayerStats().inventory;
    }

    private void Update()
    {
        // auto pick up
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.droppedItem &&
            !inventory.hasItem() &&
            autoPickEnabled
        )
        {
            // pick up dropped item
            Debug.Log(pickedItem);
            Item _item = pickedItem.GetComponent<CollectableItem>().itemMeta;
            inventory.SetItem (_item);
            thoughtBubbleRenderer.sprite = _item.thoughtBubbleSprite;
            thoughtBubbleRenderer.enabled = true;
            Destroy (pickedItem);
        }
    }

    public void OnUse()
    {
        if (inventory.hasItem())
        {
            Item currentItem = inventory.GetCurrentItem();
            Vector2 idleDirection = playerController.GetIdleDirection();
            if (currentItem.itemType == Item.ItemType.swabStick)
            {
                inventory.useItem();
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
                    inventory.useItem();
                    inventory.SetItem (trash);
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
                    inventory.useItem();
                    thoughtBubbleRenderer.enabled = false;

                    // Gain 1 coin!
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
                    inventory.useItem();
                    thoughtBubbleRenderer.enabled = false;
                    testStationProcessor.OnLock (gameObject);
                }
            }
        }
    }

    public void onPickUpDrop()
    {
        if (!inventory.hasItem())
        {
            if (
                playerZoneManager.GetZone() !=
                PlayerZoneManager.ZoneType.nullType
            )
            {
                if (
                    playerZoneManager.GetZone() ==
                    PlayerZoneManager.ZoneType.swabStickCollection
                )
                {
                    inventory.SetItem (swabStick);
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
            !inventory.hasItem() &&
            playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.shop
        )
        {
            ShopItem boughtItem = shopHandler.BuyItem(gameObject);
            if (boughtItem != null)
            {
                inventory.SetItem (shopItem);
                thoughtBubbleRenderer.sprite = shopItem.thoughtBubbleSprite;
                thoughtBubbleRenderer.enabled = true;
            }
        }
    }

    private void SubmitTestSample()
    {
        testStationProcessor.OnLoadTestSample(transform.GetInstanceID());
        inventory.useItem();
        thoughtBubbleRenderer.enabled = false;
    }

    private void DropItem()
    {
        // drop item
        if (inventory.hasItem())
        {
            Item _item = inventory.useItem();
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
        inventory.SetItem (testResult);
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
        inventory.SetItem (testSample);
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
}

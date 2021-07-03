using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwabTestPlayerController : MonoBehaviour
{
    public TMP_Text playerUIIndicatorText;

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

    [Header("Game Events Binding")]
    public ParticleGameEvent dashParticleGameEvent;

    private PlayerInput playerInput;

    private Rigidbody2D rb;

    private Animator animator;

    private Vector3 rawInputMovement = Vector3.zero;

    private Vector3 finalInputMovement = Vector3.zero; // computed with deltaTime and speed

    private bool isIdle = true; // True if the character is IDLE (not moving), else false

    private bool isStunned = false; // True if the character is tunned

    private Vector2 direction = Vector2.down;

    private Vector2 idleDirection = Vector2.down; // The direction when character is idle

    private Vector2 dashDirection = Vector2.right; // The dashed direction of the most recent dash

    private bool isDashing = false; // Whether character is currently dashing

    private bool autoPickEnabled = true;

    public TestSampleProcessor testStationProcessor; // the test station where the player is at

    public ShopHandler shopHandler;

    public GameObject pickedItem; // the item player picked up

    private PlayerInventory inventory;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController =
            playerStatsManager.GetPlayerStats().animatorController;
        inventory = playerStatsManager.GetPlayerStats().inventory;
    }

    private void OnDisable()
    {
        isStunned = true;
    }

    private void OnEnable()
    {
        isStunned = false;
        playerInput = GetComponent<PlayerInput>();
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
        playerUIIndicatorText.text =
            string.Format("{0}P", playerStatsManager.GetPlayerStats().playerID);
        playerUIIndicatorText.color =
            playerStatsManager.GetPlayerStats().playerAccent;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
    }

    private void Update()
    {
        direction = GetDirection();
        if (!(direction.x == 0 && direction.y == 0)) idleDirection = direction;
        finalInputMovement =
            rawInputMovement * Time.deltaTime * constants.playerMoveSpeed;
        transform.Translate(finalInputMovement, Space.World);
        animator.SetFloat("horizontal_idle", idleDirection.x);
        animator.SetFloat("vertical_idle", idleDirection.y);
        animator.SetFloat("horizontal", finalInputMovement.x);
        animator.SetFloat("vertical", finalInputMovement.y);
        animator.SetFloat("speed", finalInputMovement.magnitude);
        if (finalInputMovement.magnitude == 0) isIdle = true;

        // auto pick up
        if (
            playerZoneManager.GetZone() ==
            PlayerZoneManager.ZoneType.droppedItem &&
            !inventory.hasItem() &&
            autoPickEnabled
        )
        {
            // pick up dropped item
            Item _item = pickedItem.GetComponent<CollectableItem>().itemMeta;
            inventory.SetItem (_item);
            thoughtBubbleRenderer.sprite = _item.thoughtBubbleSprite;
            thoughtBubbleRenderer.enabled = true;
            Destroy (pickedItem);
        }
    }

    // Call this after "READY-START" UI finished playing, controlled by Game Event
    public void EnablePlayerMovement()
    {
        isStunned = false; // set to false so that player can move
    }

    private Vector2 GetDirection()
    {
        if (
            Math.Abs(rawInputMovement.x) > Math.Abs(rawInputMovement.y) &&
            rawInputMovement.x > 0
        )
        {
            return Vector2.right;
        }
        if (
            Math.Abs(rawInputMovement.x) > Math.Abs(rawInputMovement.y) &&
            rawInputMovement.x < 0
        )
        {
            return Vector2.left;
        }
        if (
            Math.Abs(rawInputMovement.x) < Math.Abs(rawInputMovement.y) &&
            rawInputMovement.y > 0
        )
        {
            return Vector2.up;
        }
        if (
            Math.Abs(rawInputMovement.x) < Math.Abs(rawInputMovement.y) &&
            rawInputMovement.y < 0
        )
        {
            return Vector2.down;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public void OnShow()
    {
        playerUIIndicatorText.text =
            string.Format("{0}P", playerStatsManager.GetPlayerStats().playerID);
        playerUIIndicatorText.color =
            playerStatsManager.GetPlayerStats().playerAccent;
    }

    public void OnMove(InputValue context)
    {
        if (!isStunned)
        {
            isIdle = false;
            Vector2 movement = context.Get<Vector2>();
            rawInputMovement =
                new Vector3(movement.x, movement.y, transform.position.z);
        }
    }

    public void OnDash()
    {
        if (!isStunned)
        {
            if (!isDashing && !isIdle)
            {
                isDashing = true;
                dashParticleGameEvent
                    .Fire(ParticleManager.ParticleTag.dash,
                    transform.position -
                    Vector3.up * constants.dashParticleOffset);
                dashDirection = direction;

                // remember the most recent dash direction for removal
                if (!isIdle)
                {
                    rb
                        .AddForce(dashDirection * constants.playerDashSpeed,
                        ForceMode2D.Impulse);
                }
                isDashing = false;
            }
        }
    }

    public void OnUse()
    {
        if (!isStunned)
        {
            if (inventory.hasItem())
            {
                Item currentItem = inventory.GetCurrentItem();
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
                                    testStationProcessor
                                        .testStationInfo
                                        .resultOwner
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
                        thoughtBubbleRenderer.sprite =
                            trash.thoughtBubbleSprite;
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
    }

    public void OnPickdrop()
    {
        if (!isStunned)
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
                                    testStationProcessor
                                        .testStationInfo
                                        .resultOwner
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
    }

    IEnumerator EnableAutoPickUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        autoPickEnabled = true;
    }

    public void OnShop()
    {
        if (!isStunned)
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
        isStunned = true;
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
        isStunned = false;
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
}

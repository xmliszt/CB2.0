using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLobbyPlayerController : MonoBehaviour
{
    public TMP_Text playerUIIndicatorText;

    public GameConstants constants;

    [Header("Item Scriptable Objects")]

    public Item swabStick;

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

    private PlayerStats playerStats;
    private Rigidbody2D rb;

    private Animator animator;

    private Vector3 rawInputMovement = Vector3.zero;

    private Vector3 finalInputMovement = Vector3.zero; // computed with deltaTime and speed

    private bool isIdle = true; // True if the character is IDLE (not moving), else false

    private bool isDisabled = false; // True if the character is tunned

    private Vector2 direction = Vector2.down;

    private Vector2 idleDirection = Vector2.down; // The direction when character is idle

    private Vector2 dashDirection = Vector2.right; // The dashed direction of the most recent dash

    private bool isDashing = false; // Whether character is currently dashing

    private bool autoPickEnabled = true;

    private ZoneType zoneType = ZoneType.nullType;

    private enum ZoneType
    {
        droppedItem = 0,
        swabStickCollection = 1,
        testStation = 2,
        submissionStation = 3,
        dustbin = 4,
        shop = 5,
        nullType = 6
    }

    private TestSampleProcessor testStationProcessor; // the test station where the player is at

    private GameObject pickedItem; // the item player picked up

    private ShopHandler shopHandler;

    private PlayerInventory inventory;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController =
            playerStats.animatorController;
        inventory = playerStats.inventory;
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnDisable() {
        isDisabled = true;
    }

    private void OnEnable() {
        isDisabled = false; // player can only move when the UI "READY-START" has finished playing
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
        playerUIIndicatorText.text =
            string.Format("{0}P", playerStats.playerID);
        playerUIIndicatorText.color = playerStats.playerAccent;
        GetComponent<SpriteOutlined>().EnableOutline(playerStats);
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
            zoneType == ZoneType.droppedItem &&
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

    public void SetPlayerStats(PlayerStats _playerStats)
    {  
        playerStats = _playerStats;
    }

    // Call this after "READY-START" UI finished playing, controlled by Game Event
    public void EnablePlayerMovement()
    {
        isDisabled = false; // set to false so that player can move
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

    public void OnMove(InputValue context)
    {
        if (!isDisabled)
        {
            isIdle = false;
            Vector2 movement = context.Get<Vector2>();
            rawInputMovement =
                new Vector3(movement.x, movement.y, transform.position.z);
        }
    }

    public void OnDash()
    {
        if (!isDisabled)
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
        if (!isDisabled)
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
            }
        }
    }

    public void OnPickdrop()
    {
        if (!isDisabled)
        {
            if (!inventory.hasItem())
            {
                if (zoneType != ZoneType.nullType)
                {
                    if (zoneType == ZoneType.swabStickCollection)
                    {
                        inventory.SetItem (swabStick);
                        thoughtBubbleRenderer.sprite =
                            swabStick.thoughtBubbleSprite;
                        thoughtBubbleRenderer.enabled = true;
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
        if (!isDisabled)
        {
            if (!inventory.hasItem() && zoneType == ZoneType.shop)
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

    public void SetZone(string zoneTag, GameObject zoneObject)
    {
        switch (zoneTag)
        {
            case "CollectionPoint":
                zoneType = ZoneType.swabStickCollection;
                break;
            case "Shop":
                zoneType = ZoneType.shop;
                shopHandler = zoneObject.GetComponent<ShopHandler>();
                break;
            case "SwabStick":
                GetStunned();
                break;
            case "Item":
                zoneType = ZoneType.droppedItem;
                pickedItem = zoneObject;
                break;
            case "null":
                zoneType = ZoneType.nullType;
                break;
        }
    }

    private void GetStunned()
    {
        DropItem();
        autoPickEnabled = false;
        StartCoroutine(EnableAutoPickUp(constants.playerStunnedDuration));
        stunnedIconRenderer.enabled = true;
        SpriteRenderer _renderer = GetComponent<SpriteRenderer>();
        isDisabled = true;
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
        isDisabled = false;
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
    }
}

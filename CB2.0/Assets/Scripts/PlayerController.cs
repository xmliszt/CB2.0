using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;

    public GameConstants constants;

    public PlayerInventory inventory;

    [Header("Item Types")]
    public Item swabStick;

    public Item testSample;

    public Item testResult;

    public Item trash;

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

    private Vector2 direction = Vector2.down;

    private Vector2 idleDirection = Vector2.down; // The direction when character is idle

    private Vector2 dashDirection = Vector2.right; // The dashed direction of the most recent dash

    private bool isDashing = false; // Whether character is currently dashing

    private ZoneType zoneType = ZoneType.nullType;

    private enum ZoneType
    {
        droppedItem = 0,
        swabStickCollection = 1,
        testStation = 2,
        submissionStation = 3,
        dustbin = 4,
        nullType = 5
    }

    private TestSampleProcessor testStationProcessor; // the test station where the player is at

    private GameObject pickedItem; // the item player picked up

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        GetComponent<Animator>().runtimeAnimatorController =
            playerStats.animatorController;
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
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

    public void OnMove(InputAction.CallbackContext context)
    {
        isIdle = false;
        Vector2 movement = context.ReadValue<Vector2>();
        rawInputMovement =
            new Vector3(movement.x, movement.y, transform.position.z);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing && !isIdle)
        {
            isDashing = true;
            dashParticleGameEvent
                .Fire(ParticleManager.ParticleTag.dash,
                transform.position - Vector3.up * constants.dashParticleOffset);
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

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.started)
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
                    if (zoneType == ZoneType.testStation)
                    {
                        if (!testStationProcessor.testStationInfo.isLoaded)
                        {
                            testStationProcessor
                                .OnLoadTestSample(transform.GetInstanceID());
                            inventory.useItem();
                            thoughtBubbleRenderer.enabled = false;
                        }
                    }
                }
                else if (currentItem.itemType == Item.ItemType.testResult)
                {
                    // submit test result to submission desk
                    if (zoneType == ZoneType.submissionStation)
                    {
                        inventory.useItem();
                        inventory.SetItem (trash);
                        thoughtBubbleRenderer.sprite =
                            trash.thoughtBubbleSprite;
                        thoughtBubbleRenderer.enabled = true;
                        Debug.Log("Submit Test Result!");

                        // Log 1 completed swab test
                        playerStats.completedSwabTests++;
                    }
                }
                else if (currentItem.itemType == Item.ItemType.trash)
                {
                    // throw the trash and gain coins
                    if (zoneType == ZoneType.dustbin)
                    {
                        inventory.useItem();
                        thoughtBubbleRenderer.enabled = false;
                        Debug.Log("Throw Away Trash!");

                        // Gain 1 coin!
                        playerStats.coins++;
                    }
                }
            }
        }
    }

    public void OnPickDrop(InputAction.CallbackContext context)
    {
        if (context.started)
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
                    else if (zoneType == ZoneType.testStation)
                    {
                        if (testStationProcessor.testStationInfo.isComplete)
                        {
                            if (testStationProcessor.testStationInfo.isLocked)
                            {
                                if (
                                    transform.GetInstanceID() ==
                                    testStationProcessor
                                        .testStationInfo
                                        .resultOwner[0]
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
                    else if (zoneType == ZoneType.droppedItem)
                    {
                        // pick up dropped item
                        Item _item =
                            pickedItem.GetComponent<CollectableItem>().itemMeta;
                        inventory.SetItem (_item);
                        thoughtBubbleRenderer.sprite =
                            _item.thoughtBubbleSprite;
                        thoughtBubbleRenderer.enabled = true;
                        Destroy(pickedItem);
                    }
                }
            }
            else
            {
                DropItem();
            }
        }
    }

    private void DropItem()
    {
        // drop item
        if (inventory.hasItem())
        {
            Item _item = inventory.useItem();
            GameObject dropped =
                Instantiate(droppedItemPrefab,
                transform.position + new Vector3(idleDirection.x, idleDirection.y, droppedItemPrefab.transform.position.z) * 0.5f,
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

    public void OnShop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Player shop(buy) item");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "CollectionPoint":
                zoneType = ZoneType.swabStickCollection;
                break;
            case "Dustbin":
                zoneType = ZoneType.dustbin;
                break;
            case "TestStation":
                zoneType = ZoneType.testStation;
                testStationProcessor =
                    other.gameObject.GetComponent<TestSampleProcessor>();
                break;
            case "SubmissionDesk":
                zoneType = ZoneType.submissionStation;
                break;
            case "SwabStick":
                GetStunned();
                break;
            case "Item":
                zoneType = ZoneType.droppedItem;
                pickedItem = other.gameObject;
                break;
        }
    }

    private void GetStunned()
    {
        DropItem();
        stunnedIconRenderer.enabled = true;
        GetComponent<PlayerInput>().enabled = false;
        SpriteRenderer _renderer = GetComponent<SpriteRenderer>();
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
        GetComponent<PlayerInput>().enabled = true;
        stunnedIconRenderer.enabled = false;
        SpriteRenderer _renderer = GetComponent<SpriteRenderer>();
        _renderer.color =
            new Color(_renderer.color.r,
                _renderer.color.g,
                _renderer.color.b,
                1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        zoneType = ZoneType.nullType;
    }

    public void OnSwabStickHit()
    {
        inventory.SetItem (testSample);
        thoughtBubbleRenderer.sprite = testSample.thoughtBubbleSprite;
        thoughtBubbleRenderer.enabled = true;
    }
}

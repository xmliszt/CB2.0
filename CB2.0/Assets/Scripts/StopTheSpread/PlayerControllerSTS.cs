using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControllerSTS : MonoBehaviour
{
    public PlayerStats playerStats;

    public GameConstants constants;

    public STSInteractables stsInteractables;

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

    [SerializeField] private Slider completionBar;

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

    private bool autoPickEnabled = true;

    private ZoneType zoneType = ZoneType.nullType;

    private bool canMove = true; // boolean to stop the player from moving

    private bool playerDoingActivity = false;



    private enum ZoneType
    {
        droppedItem = 0,
        swabStickCollection = 1,
        testStation = 2,
        submissionStation = 3,
        dustbin = 4,
        shop = 5,
        nullType = 6,
        interactable = 7,

        dumbbell = 10,
        computer = 11,
        karaoke = 12,
        food = 13,
        grocer = 14,
        NPCCustomer = 15
    }

    private TestSampleProcessor testStationProcessor; // the test station where the player is at

    private GameObject pickedItem; // the item player picked up

    private ShopHandler shopHandler;

    private PlayerInventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        inventory = playerStats.inventory;
        GetComponent<Animator>().runtimeAnimatorController =
            playerStats.animatorController;
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);
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
        if (zoneType == ZoneType.droppedItem && !inventory.hasItem() && autoPickEnabled)
        {
            // pick up dropped item
            Item _item = pickedItem.GetComponent<CollectableItem>().itemMeta;
            inventory.SetItem(_item);
            thoughtBubbleRenderer.sprite = _item.thoughtBubbleSprite;
            thoughtBubbleRenderer.enabled = true;
            Destroy(pickedItem);
        }
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
        if (canMove)
        {
            isIdle = false;
            Vector2 movement = context.Get<Vector2>();
            rawInputMovement =
                new Vector3(movement.x, movement.y, transform.position.z);
        }
    }

    public void OnDash()
    {
        if (!isDashing && !isIdle)
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

    IEnumerator UsingInteractable()
    {
        completionBar.gameObject.SetActive(true);
        playerDoingActivity = true;

        // wait for 3 seconds, check every 0.5s if the person is out of the box
        for (int i = 0; i < stsInteractables.intervals; i++)
        {
            completionBar.value += stsInteractables.doingActivityInterval;
            yield return new WaitForSeconds(stsInteractables.doingActivityInterval);
            if (zoneType == ZoneType.nullType)
            {
                Debug.Log("Player stopped halfway");
                playerDoingActivity = false;

                completionBar.value = 0;
                completionBar.gameObject.SetActive(false);

                yield break;
            }
        }

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);
        playerDoingActivity = false;
        Debug.Log("Player completed activity");
    }

    public void OnUse()
    {
        // should be using Enum.IsDefined, but then cause cannot check Enum of different type so using this inefficient method as of now
        if (zoneType == ZoneType.dumbbell || zoneType == ZoneType.computer || zoneType == ZoneType.karaoke || zoneType == ZoneType.food || zoneType == ZoneType.grocer || zoneType == ZoneType.NPCCustomer)
        {
            if (!playerDoingActivity)
            {
                Debug.Log("Using activity");
                StartCoroutine(UsingInteractable());
            }
        }

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
                if (zoneType == ZoneType.submissionStation)
                {
                    inventory.useItem();
                    inventory.SetItem (trash);
                    thoughtBubbleRenderer.sprite = trash.thoughtBubbleSprite;
                    thoughtBubbleRenderer.enabled = true;

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

                    // Gain 1 coin!
                    playerStats.coins += constants.coinAwardedPerCompleteTest;
                }
            }
            else if (currentItem.itemType == Item.ItemType.shopItem)
            {
                if (zoneType == ZoneType.testStation)
                {
                    inventory.useItem();
                    thoughtBubbleRenderer.enabled = false;
                    testStationProcessor.OnLock (gameObject);
                }
            }
        }
    }

    public void OnPickdrop()
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
            StartCoroutine(EnableAutoPickUp());
        }
    }

    IEnumerator EnableAutoPickUp()
    {
        yield return new WaitForSeconds(1);
        autoPickEnabled = true;
    }

    public void OnShop()
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

    public void PlayerOnTriggerEnterInteractable(String otherTag)
    {
        switch (otherTag)
        {
            case "Dumbbell":
                zoneType = ZoneType.dumbbell;
                break;
            case "Computer":
                zoneType = ZoneType.computer;
                break;
            case "Karaoke":
                zoneType = ZoneType.karaoke;
                break;
            case "Food":
                zoneType = ZoneType.food;
                break;
            case "Grocer":
                zoneType = ZoneType.grocer;
                break;
            case "NPCCustomer":
                zoneType = ZoneType.NPCCustomer;
                break;
        }
    }

    public void PlayerOnTriggerExitInteractable()
    {
        zoneType = ZoneType.nullType;
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

    /*private void OnTriggerExit2D(Collider2D other)
    {
        zoneType = ZoneType.nullType;
    }*/

    public void OnSwabStickHit()
    {
        inventory.SetItem (testSample);
        thoughtBubbleRenderer.sprite = testSample.thoughtBubbleSprite;
        thoughtBubbleRenderer.enabled = true;
    }
}

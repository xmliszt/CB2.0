using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;

    public Vector2Variable playerFacingDirection;

    [Header("Game Events Binding")]
    public ParticleGameEvent dashParticleGameEvent;

    public SingleIntegerGameEvent onSubmitTestSample;

    public SingleIntegerGameEvent onRetrieveTestResult;

    public SingleIntegerGameEvent onBeforeSubmitTestSampleCheckStation;

    public SingleIntegerGameEvent onBeforeRetrieveTestResultCheckStation;

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

    private List<int> swabStickIDs; // Collection of ID of swab stick object fired by the character and has yet to be destroyed

    private ZoneType zoneType = ZoneType.nullType;

    private enum ZoneType
    {
        swabStickCollection = 1,
        testStation = 2,
        submissionStation = 3,
        dustbin = 4,
        nullType = 5
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        swabStickIDs = new List<int>();
        thoughtBubbleRenderer.enabled = false;
    }

    private void Update()
    {
        direction = GetDirection();
        if (!(direction.x == 0 && direction.y == 0)) idleDirection = direction;
        playerFacingDirection.Set (idleDirection);
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
            if (!isIdle) rb.velocity += direction * constants.playerDashSpeed;
            StartCoroutine(removeDash());
        }
    }

    IEnumerator removeDash()
    {
        yield return new WaitForSeconds(constants.playerDashDuration);
        rb.velocity -= dashDirection * constants.playerDashSpeed;
        isDashing = false;
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
                    swabStickIDs.Add(stick.transform.GetInstanceID());
                    thoughtBubbleRenderer.enabled = false;
                }
                else if (currentItem.itemType == Item.ItemType.testSample)
                {
                    // submit test sample to test station
                    if (zoneType == ZoneType.testStation)
                    {
                        onBeforeSubmitTestSampleCheckStation
                            .Fire(transform.GetInstanceID());
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
                        playerStats.completedSwabTests ++;
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
                        playerStats.coins ++;
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
                        onBeforeRetrieveTestResultCheckStation
                            .Fire(transform.GetInstanceID());
                    }
                }
            }
        }
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
                break;
            case "SubmissionDesk":
                zoneType = ZoneType.submissionStation;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        zoneType = ZoneType.nullType;
    }

    public void OnSwabStickHit(int swabStickID)
    {
        if (swabStickIDs.Contains(swabStickID))
        {
            inventory.SetItem (testSample);
            thoughtBubbleRenderer.sprite = testSample.thoughtBubbleSprite;
            thoughtBubbleRenderer.enabled = true;
            swabStickIDs.Remove (swabStickID);
        }
    }

    public void OnBeforeCollectTestResultsStationReply(
        TestStation testStationInfo
    )
    {
        if (testStationInfo.isComplete)
        {
            if (testStationInfo.isLocked)
            {
                if (transform.GetInstanceID() == testStationInfo.resultOwner[0])
                {
                    CollectResult();
                }
            }
            else
            {
                CollectResult();
            }
        }
    }

    private void CollectResult()
    {
        inventory.SetItem (testResult);
        thoughtBubbleRenderer.sprite = testResult.thoughtBubbleSprite;
        thoughtBubbleRenderer.enabled = true;
        onRetrieveTestResult.Fire(transform.GetInstanceID());
    }

    public void OnBeforeSubmitTestSampleStationReply(
        TestStation testStationInfo
    )
    {
        if (!testStationInfo.isLoaded)
        {
            inventory.useItem();
            thoughtBubbleRenderer.enabled = false;
            onSubmitTestSample.Fire(transform.GetInstanceID());
            Debug.Log("Submit Test Sample!");
        }
    }
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLobbyPlayerController : MonoBehaviour
{
    

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
    public SingleIntegerGameEvent onPlayerChangeProfile;

    public ParticleGameEvent dashParticleGameEvent;

    private PlayerInput playerInput;

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

    private TestSampleProcessor testStationProcessor; // the test station where the player is at

    private GameObject pickedItem; // the item player picked up

    private ShopHandler shopHandler;

    private PlayerInventory inventory;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = playerStatsManager.GetPlayerStats().inventory;
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnDisable()
    {
        isDisabled = true;
    }

    private void OnEnable()
    {
        isDisabled = false; // player can only move when the UI "READY-START" has finished playing
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
    }

    private void Update()
    {
        animator.runtimeAnimatorController =
            playerStatsManager.GetPlayerStats().animatorController;
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
            Debug.Log(playerZoneManager.GetZone());
            if (
                playerZoneManager.GetZone() ==
                PlayerZoneManager.ZoneType.clothChanger
            )
            {
                onPlayerChangeProfile
                    .Fire(playerStatsManager.GetPlayerStats().playerID);
            }
        }
    }

    public void OnPickdrop()
    {
        if (!isDisabled)
        {
        }
    }

    public void OnShop()
    {
        if (!isDisabled)
        {
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameStats gameStats;

    public GameConstants constants;

    public ParticleGameEvent dashParticleGameEvent;

    private PlayerInput playerInput;

    private PlayerStatsManager playerStatsManager;

    private Rigidbody2D rb;

    private Animator animator;

    private Vector3 rawInputMovement = Vector3.zero;

    private Vector3 finalInputMovement = Vector3.zero; // computed with deltaTime and speed

    private bool isIdle = true; // True if the character is IDLE (not moving), else false

    private bool disabled = false; // True if the character is tunned

    private Vector2 direction = Vector2.down;

    private Vector2 idleDirection = Vector2.down; // The direction when character is idle

    private Vector2 dashDirection = Vector2.right; // The dashed direction of the most recent dash

    private bool isDashing = false; // Whether character is currently dashing

    // All minigame handlers
    private GameLobbyControlHandler gameLobbyControlHandler;

    private SwabTestControlHandler swabTestControlHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        swabTestControlHandler = GetComponent<SwabTestControlHandler>();
        gameLobbyControlHandler = GetComponent<GameLobbyControlHandler>();
    }

    private void Start()
    {
        animator.runtimeAnimatorController =
            playerStatsManager.GetPlayerStats().animatorController;
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
        if (!disabled)
        {
            isIdle = false;
            Vector2 movement = context.ReadValue<Vector2>();
            rawInputMovement =
                new Vector3(movement.x, movement.y, transform.position.z);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled)
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
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.gameLobby:
                        gameLobbyControlHandler.OnUse();
                        break;
                    case GameStats.Scene.swabTestWar:
                        swabTestControlHandler.OnUse();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void OnPickdrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.swabTestWar:
                        swabTestControlHandler.onPickUpDrop();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void OnShop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.swabTestWar:
                        swabTestControlHandler.onShop();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public Vector2 GetIdleDirection()
    {
        return idleDirection;
    }

    public void EnableController()
    {
        disabled = false;
    }

    public void DisableController()
    {
        disabled = true;
    }
}

using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameStats gameStats;

    public GameConstants constants;

    public GameEvent onGamePaused;

    public ParticleGameEvent dashParticleGameEvent;

    private PlayerInput playerInput;

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
    private PlayerStatsManager playerStatsManager;

    private PlayerAudioController playerAudioController;

    private GameLobbyControlHandler gameLobbyControlHandler;

    private PlayerReadyHandler playerReadyHandler;

    private SwabTestControlHandler swabTestControlHandler;

    private SnHPlayerControlHandler snHPlayerControlHandler;

    private STSControlHandler stsControlHandler;

    private UnlimitedGroupControlHandler unlimitedGroupControlHandler;

    private float movementFactor = 1.0f; // used to stop or resume movement of character. 0 will stop, 1 will resume

    private float speedFactor = 1.0f; // for UGS to change player speed

    private bool dashDisabled = false;

    private bool isPausedExecuted = false;

    private void Awake()
    {
        DontDestroyOnLoad (gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerReadyHandler = GetComponent<PlayerReadyHandler>();
        playerAudioController = GetComponent<PlayerAudioController>();
        swabTestControlHandler = GetComponent<SwabTestControlHandler>();
        snHPlayerControlHandler = GetComponent<SnHPlayerControlHandler>();
        stsControlHandler = GetComponent<STSControlHandler>();
        gameLobbyControlHandler = GetComponent<GameLobbyControlHandler>();
        unlimitedGroupControlHandler =
            GetComponent<UnlimitedGroupControlHandler>();
        animator.runtimeAnimatorController =
            playerStatsManager.GetPlayerStats().animatorController;
    }

    private void Update()
    {
        direction = GetDirection();
        if (!(direction.x == 0 && direction.y == 0)) idleDirection = direction;
        finalInputMovement =
            rawInputMovement *
            Time.deltaTime *
            constants.playerMoveSpeed *
            speedFactor *
            movementFactor;
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

    [PunRPC]
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

    [PunRPC]
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled && !dashDisabled)
            {
                if (!isDashing && !isIdle)
                {
                    if (playerAudioController)
                        playerAudioController.PlaySFX(SFXType.dash);
                    isDashing = true;

                    dashDirection = direction;

                    // remember the most recent dash direction for removal
                    if (!isIdle && rb)
                    {
                        rb
                            .AddForce(dashDirection * constants.playerDashSpeed,
                            ForceMode2D.Impulse);
                        dashParticleGameEvent
                            .Fire(ParticleManager.ParticleTag.dash,
                            transform.position -
                            Vector3.up * constants.dashParticleOffset);
                    }
                    isDashing = false;
                }
            }
        }
    }

    [PunRPC]
    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.gameLobby:
                        if (gameLobbyControlHandler)
                            gameLobbyControlHandler.OnUse();
                        break;
                    case GameStats.Scene.swabTestWar:
                        if (swabTestControlHandler)
                            swabTestControlHandler.OnUse();
                        break;
                    case GameStats.Scene.stopTheSpread:
                        if (stsControlHandler) stsControlHandler.OnUse();
                        break;
                    case GameStats.Scene.unlimitedGroupSize:
                        if (unlimitedGroupControlHandler)
                            unlimitedGroupControlHandler.OnUse();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [PunRPC]
    public void OnPickdrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (playerReadyHandler)
            {
                playerReadyHandler.OnPlayerReady();
            }
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.swabTestWar:
                        if (swabTestControlHandler)
                            swabTestControlHandler.onPickUpDrop();
                        break;
                    case GameStats.Scene.stopTheSpread:
                        if (stsControlHandler) stsControlHandler.onPickUpDrop();
                        break;
                    case GameStats.Scene.unlimitedGroupSize:
                        if (unlimitedGroupControlHandler)
                            unlimitedGroupControlHandler.OnPickUpDrop();
                        break;
                    case GameStats.Scene.snatchAndHoard:
                        if (snHPlayerControlHandler)
                            snHPlayerControlHandler.OnPickDropItem();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [PunRPC]
    public void OnShop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.swabTestWar:
                        if (swabTestControlHandler)
                            swabTestControlHandler.onShop();
                        break;
                    case GameStats.Scene.stopTheSpread:
                        if (stsControlHandler) stsControlHandler.onShop();
                        break;
                    case GameStats.Scene.unlimitedGroupSize:
                        if (unlimitedGroupControlHandler)
                            unlimitedGroupControlHandler.OnShop();
                        break;
                    case GameStats.Scene.snatchAndHoard:
                        if (snHPlayerControlHandler)
                            snHPlayerControlHandler.OnShop();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [PunRPC]
    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!disabled)
            {
                switch (gameStats.GetCurrentScene())
                {
                    case GameStats.Scene.unlimitedGroupSize:
                        if (unlimitedGroupControlHandler)
                            unlimitedGroupControlHandler.OnHold(context);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [PunRPC]
    public void onPause(InputAction.CallbackContext context)
    {
        if (!disabled)
        {
            if (context.performed && !isPausedExecuted)
            {
                if (playerAudioController)
                    playerAudioController.PlaySFX(SFXType._lock);
                StartCoroutine(removePausedExecuted());
                isPausedExecuted = true;
                onGamePaused.Fire();
            }
        }
    }

    IEnumerator removePausedExecuted()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        isPausedExecuted = false;
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

    public void DisableMovement()
    {
        movementFactor = 0;
    }

    public void EnableMovement()
    {
        movementFactor = 1.0f;
    }

    public void SlowMovement(float factor)
    {
        speedFactor = factor;
    }

    public void SpeedUpMovement(float factor)
    {
        speedFactor = factor;
    }

    public void RestoreMovement()
    {
        speedFactor = 1.0f;
    }

    public void DisableDash()
    {
        dashDisabled = true;
    }

    public void EnableDash()
    {
        dashDisabled = false;
    }
}

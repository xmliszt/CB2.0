using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameConstants constants;

    private PlayerInput playerInput;

    private Rigidbody2D rb;

    private Animator animator;

    private Vector3 rawInputMovement = Vector3.zero;

    private Vector3 finalInputMovement = Vector3.zero;

    private bool isIdle = true; // True if the character is IDLE (not moving), else false

    private Vector2 direction = Vector2.right; // The direction that the character is facing

    private Vector2 dashDirection = Vector2.right; // The dashed direction of the most recent dash

    private bool isDashing = false; // Whether character is currently dashing

    private bool picked = false; // A temporary variable to set up item picked/dropped status for the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        direction = GetDirection();
        finalInputMovement =
            rawInputMovement * Time.deltaTime * constants.playerMoveSpeed;
        transform.Translate(finalInputMovement, Space.World);
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
        else
        {
            return Vector2.down;
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
            ParticleManager._instance.PlayParticle(ParticleManager.ParticleTag.dash, transform.position - Vector3.up * 0.4f);
            dashDirection = direction; // remember the most recent dash direction for removal
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
            if (picked)
            {
                Debug.Log("Player Use Item");
            }
            else
            {
                Debug.Log("Nothing to use!");
            }
        }
    }

    public void OnPickDrop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!picked)
            {
                picked = true;
                Debug.Log("Player Pick Up Item");
            }
            else
            {
                picked = false;
                Debug.Log("Player Drop Item");
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
}

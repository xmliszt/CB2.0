using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameConstants constants;

    private PlayerInput playerInput;

    private Vector3 rawInputMovement = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        transform
            .Translate(rawInputMovement *
            Time.deltaTime *
            constants.playerMoveSpeed,
            Space.World);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        rawInputMovement =
            new Vector3(movement.x, movement.y, transform.position.z);
    }
}

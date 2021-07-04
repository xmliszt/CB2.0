using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public void OnDeviceLost(PlayerInput playerInput)
    {
        Debug.Log(string.Format("Player {0} device lost", playerInput.playerIndex + 1));
    }
}

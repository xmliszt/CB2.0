using UnityEngine;
public class PlayerInputHandler : MonoBehaviour
{
    public void OnDeviceLost()
    {
        Destroy(gameObject);
    }
}

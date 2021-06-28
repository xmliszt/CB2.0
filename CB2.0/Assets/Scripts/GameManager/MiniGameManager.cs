using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public PlayerInventory inventory;
    void Start()
    {
        inventory.ClearItem();
    }
}

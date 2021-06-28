using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInventory inventory;
    void Start()
    {
        inventory.ClearItem();
    }
}

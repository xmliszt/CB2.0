using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public PlayerStats[] players;
    public TestStation[] testStations;
    public PlayerInventory[] inventories;
    void Start()
    {
        foreach (PlayerInventory inventory in inventories)
        {
            inventory.ClearItem();
        }
        foreach (TestStation testStation in testStations)
        {
            testStation.isLoaded = false;
            testStation.isLocked = false;
            testStation.isComplete = false;
            testStation.resultOwner = new List<int>();
            testStation.playersInZone = new List<int>();
        }
        foreach (PlayerStats player in players)
        {
            player.coins = 0;
            player.completedSwabTests = 0;
        }
    }
}

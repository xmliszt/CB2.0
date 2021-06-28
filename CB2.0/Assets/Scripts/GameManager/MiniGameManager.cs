using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public TestStation[] testStations;
    public PlayerInventory inventory;
    void Start()
    {
        inventory.ClearItem();
        foreach (TestStation testStation in testStations)
        {
            testStation.isLoaded = false;
            testStation.isLocked = false;
            testStation.isComplete = false;
            testStation.resultOwner = new List<int>();
            testStation.playersInZone = new List<int>();
        }
    }
}

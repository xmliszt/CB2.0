using System.Collections.Generic;
using UnityEngine;

public class SwabTestWarManager : MonoBehaviour
{
    public GameEvent OnStartPlayStart;
    public TestStation[] testStations;
    void Start()
    {
        OnStartPlayStart.Fire();
        foreach (TestStation testStation in testStations)
        {
            testStation.isLoaded = false;
            testStation.isLocked = false;
            testStation.isComplete = false;
            testStation.resultOwner = 0;
            testStation.playersInZone = new List<int>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "TestStation",
        menuName = "ScriptableObjects/TestStation",
        order = 5)
]
public class TestStation : ScriptableObject
{
    public int id;

    public bool isLoaded = false; // true if someone has submitted a test sample and the result has yet to be collected. false if the station can accept a new test sample submission

    public bool isComplete = false; // true if the result is ready for collection, false if no result yet
    public List<int> playersInZone;

    public int resultOwner; // The ID of the player who delivered the test sample, will be used for locking

    public bool isLocked = false; // if true, only player with ID == resultOwnerID can retrieve the result
}

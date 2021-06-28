using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "TestStationInfoGameEvent",
        menuName = "ScriptableObjects/GameEvents/TestStationInfoGameEvent",
        order = 0)
]
public class TestStationInfoGameEvent : ScriptableObject
{
    private readonly List<TestStationInfoGameEventListener>
        eventListeners = new List<TestStationInfoGameEventListener>();

    public void Fire(TestStation testStation)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(testStation);
        }
    }

    public void Register(TestStationInfoGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(TestStationInfoGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

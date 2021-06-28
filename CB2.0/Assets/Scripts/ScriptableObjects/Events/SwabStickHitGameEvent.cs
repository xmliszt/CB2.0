using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "SwabStickHitGameEvent",
        menuName = "ScriptableObjects/GameEvents/SwabStickHitGameEvent",
        order = 0)
]
public class SwabStickHitGameEvent : ScriptableObject
{
    private readonly List<SwabStickHitGameEventListener>
        eventListeners = new List<SwabStickHitGameEventListener>();

    public void Fire(int swabStickID)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(swabStickID);
        }
    }

    public void Register(SwabStickHitGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(SwabStickHitGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "SingleIntegerGameEvent",
        menuName = "ScriptableObjects/GameEvents/SingleIntegerGameEvent",
        order = 0)
]
public class SingleIntegerGameEvent : ScriptableObject
{
    private readonly List<SingleIntegerGameEventListener>
        eventListeners = new List<SingleIntegerGameEventListener>();

    public void Fire(int value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(value);
        }
    }

    public void Register(SingleIntegerGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(SingleIntegerGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

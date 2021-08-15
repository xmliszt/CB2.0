using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "IntegerStringGameEvent",
        menuName = "ScriptableObjects/GameEvents/IntegerStringGameEvent",
        order = 0)
]
public class IntegerStringGameEvent : ScriptableObject
{
    private readonly List<IntegerStringGameEventListener>
        eventListeners = new List<IntegerStringGameEventListener>();

    public void Fire(int value, string name)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(value, name);
        }
    }

    public void Register(IntegerStringGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(IntegerStringGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

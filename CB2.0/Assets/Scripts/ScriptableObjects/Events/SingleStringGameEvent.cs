using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "SingleStringGameEvent",
        menuName = "ScriptableObjects/GameEvents/SingleStringGameEvent",
        order = 0)
]
public class SingleStringGameEvent : ScriptableObject
{
    private readonly List<SingleStringGameEventListener>
        eventListeners = new List<SingleStringGameEventListener>();

    public void Fire(string value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(value);
        }
    }

    public void Register(SingleStringGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(SingleStringGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

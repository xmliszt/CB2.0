using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "BasicGameEvent",
        menuName = "ScriptableObjects/GameEvents/Vector3GameEvent",
        order = 0)
]
public class Vector3GameEvent : ScriptableObject
{
    private readonly List<Vector3GameEventListener>
        eventListeners = new List<Vector3GameEventListener>();

    public void Fire(Vector3 value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(value);
        }
    }

    public void Register(Vector3GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(Vector3GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

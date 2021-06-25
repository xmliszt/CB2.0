using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "BasicGameEvent",
        menuName = "ScriptableObjects/GameEvents/BasicGameEvent",
        order = 0)
]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener>
        eventListeners = new List<GameEventListener>();

    public void Fire()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired();
        }
    }

    public void Register(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

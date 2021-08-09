using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "MinigameTagGameEvent",
        menuName = "ScriptableObjects/GameEvents/MinigameTagGameEvent",
        order = 0)
]
public class MinigameTagGameEvent : ScriptableObject
{
    private readonly List<MinigameTagGameEventListener>
        eventListeners = new List<MinigameTagGameEventListener>();

    public void Fire(GameStats.Scene sceneType)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(sceneType);
        }
    }

    public void Register(MinigameTagGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(MinigameTagGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

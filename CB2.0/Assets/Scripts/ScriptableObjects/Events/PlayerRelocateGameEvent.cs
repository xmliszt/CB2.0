using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "PlayerRelocateGameEvent",
        menuName = "ScriptableObjects/GameEvents/PlayerRelocateGameEvent",
        order = 0)
]
public class PlayerRelocateGameEvent : ScriptableObject
{
    private readonly List<PlayerRelocateGameEventListener>
        eventListeners = new List<PlayerRelocateGameEventListener>();

    public void Fire(int playerID, Vector3 location)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(playerID, location);
        }
    }

    public void Register(PlayerRelocateGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(PlayerRelocateGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

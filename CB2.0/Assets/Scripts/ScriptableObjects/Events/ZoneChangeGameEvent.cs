using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "ZoneChangeGameEvent",
        menuName = "ScriptableObjects/GameEvents/ZoneChangeGameEvent",
        order = 0)
]
public class ZoneChangeGameEvent : ScriptableObject
{
    private readonly List<ZoneChangeGameEventListener>
        eventListeners = new List<ZoneChangeGameEventListener>();

    public void Fire(int playerID, string zoneTag, GameObject zoneObject)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(playerID, zoneTag, zoneObject);
        }
    }

    public void Register(ZoneChangeGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(ZoneChangeGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

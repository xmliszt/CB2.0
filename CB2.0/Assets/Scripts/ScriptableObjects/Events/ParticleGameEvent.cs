using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "ParticleGameEvent",
        menuName = "ScriptableObjects/GameEvents/ParticleGameEvent",
        order = 0)
]
public class ParticleGameEvent : ScriptableObject
{
    private readonly List<ParticleGameEventListener>
        eventListeners = new List<ParticleGameEventListener>();

    public void Fire(ParticleManager.ParticleTag tag, Vector3 position)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventFired(tag, position);
        }
    }

    public void Register(ParticleGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add (listener);
        }
    }

    public void Unregister(ParticleGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove (listener);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ParticleUnityEvent : UnityEvent<ParticleManager.ParticleTag, Vector3> {}

public class ParticleGameEventListener : MonoBehaviour
{
    public ParticleGameEvent Event;

    public ParticleUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }
    
    /// <param name="tag">The tag used to identify the particle system to be played</param>
    /// <param name="position">The position in Vector3 where the particle system is to be played</param>
    public void OnEventFired(ParticleManager.ParticleTag tag, Vector3 position)
    {
        Response.Invoke(tag, position);
    }
}

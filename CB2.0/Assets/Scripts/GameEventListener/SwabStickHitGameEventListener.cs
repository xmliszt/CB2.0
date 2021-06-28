using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SwabStickHitUnityEvent : UnityEvent<int> {}

public class SwabStickHitGameEventListener : MonoBehaviour
{
    public SwabStickHitGameEvent Event;

    public SwabStickHitUnityEvent Response;

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
    public void OnEventFired(int swabStickID)
    {
        Response.Invoke(swabStickID);
    }
}

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3> { }

public class Vector3GameEventListener : MonoBehaviour
{
    public Vector3GameEvent Event;

    public Vector3UnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }

    public void OnEventFired(Vector3 value)
    {
        Response.Invoke (value);
    }
}

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SingleIntegerUnityEvent : UnityEvent<int> { }

public class SingleIntegerGameEventListener : MonoBehaviour
{
    public SingleIntegerGameEvent Event;

    public SingleIntegerUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }

    public void OnEventFired(int value)
    {
        Response.Invoke (value);
    }
}

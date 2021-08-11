using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SingleStringUnityEvent : UnityEvent<string> {}

public class SingleStringGameEventListener : MonoBehaviour
{
    public SingleStringGameEvent Event;

    public SingleStringUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }
    
    public void OnEventFired(string text)
    {
        Response.Invoke(text);
    }
}

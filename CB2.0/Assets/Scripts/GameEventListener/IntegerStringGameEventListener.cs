using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntegerStringUnityEvent : UnityEvent<int, string> {}

public class IntegerStringGameEventListener : MonoBehaviour
{
    public IntegerStringGameEvent Event;

    public IntegerStringUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }
    
    public void OnEventFired(int value, string text)
    {
        Response.Invoke(value, text);
    }
}

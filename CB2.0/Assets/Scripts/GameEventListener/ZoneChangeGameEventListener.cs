using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ZoneChangeUnityEvent : UnityEvent<int, string, GameObject> { }

public class ZoneChangeGameEventListener : MonoBehaviour
{
    public ZoneChangeGameEvent Event;

    public ZoneChangeUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }

    public void OnEventFired(int playerID, string zoneTag, GameObject zoneObject)
    {
        Response.Invoke (playerID, zoneTag, zoneObject);
    }
}

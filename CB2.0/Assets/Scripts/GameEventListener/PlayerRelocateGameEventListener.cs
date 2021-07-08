using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerRelocateUnityEvent : UnityEvent<int, Vector3> { }

public class PlayerRelocateGameEventListener : MonoBehaviour
{
    public PlayerRelocateGameEvent Event;

    public PlayerRelocateUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }

    public void OnEventFired(int playerID, Vector3 location)
    {
        Response.Invoke (playerID, location);
    }
}

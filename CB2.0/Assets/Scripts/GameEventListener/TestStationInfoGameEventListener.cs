using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TestStationInfoUnityEvent : UnityEvent<TestStation> { }

public class TestStationInfoGameEventListener : MonoBehaviour
{
    public TestStationInfoGameEvent Event;

    public TestStationInfoUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }

    public void OnEventFired(TestStation testStation)
    {
        Response.Invoke (testStation);
    }
}

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MinigameTagUnityEvent : UnityEvent<GameStats.Scene> {}

public class MinigameTagGameEventListener : MonoBehaviour
{
    public MinigameTagGameEvent Event;

    public MinigameTagUnityEvent Response;

    private void OnEnable()
    {
        Event.Register(this);
    }

    private void OnDisable()
    {
        Event.Unregister(this);
    }
    
    public void OnEventFired(GameStats.Scene sceneType)
    {
        Response.Invoke(sceneType);
    }
}

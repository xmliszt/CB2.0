using UnityEngine;

[CreateAssetMenu(fileName = "STSInteractables", menuName = "ScriptableObjects/STS/Interactables", order = 1)]
public class STSInteractables : ScriptableObject
{
    public int maxSliderVal = 3;
    public float doingActivityInterval = 0.25f;
    public int intervals = 12; // intervals = maxSliderVal / doingActivityInterval
}

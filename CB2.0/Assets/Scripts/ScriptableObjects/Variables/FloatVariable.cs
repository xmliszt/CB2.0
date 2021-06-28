using UnityEngine;

[
    CreateAssetMenu(
        fileName = "FloatVar",
        menuName = "ScriptableObjects/Variables/FloatVar",
        order = 2)
]
public class FloatVariable : ScriptableObject
{
    private float _value = 0.0f;

    public float Value
    {
        get
        {
            return _value;
        }
    }

    public void Increment(float value)
    {
        _value += value;
    }

    public void Decrement(float value)
    {
        _value -= value;
    }

    public void Set(float value)
    {
        _value = value;
    }

    public void Set(FloatVariable value)
    {
        _value = value.Value;
    }
}

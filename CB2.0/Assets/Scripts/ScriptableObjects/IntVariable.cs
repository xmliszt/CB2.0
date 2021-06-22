using UnityEngine;

[
    CreateAssetMenu(
        fileName = "IntVar",
        menuName = "ScriptableObjects/IntVar",
        order = 1)
]
public class IntVariable : ScriptableObject
{
    private int _value = 0;

    public int Value
    {
        get
        {
            return _value;
        }
    }

    public void Increment(int value)
    {
        _value += value;
    }

    public void Decrement(int value)
    {
        _value -= value;
    }

    public void Set(int value)
    {
        _value = value;
    }

    public void Set(IntVariable value)
    {
        _value = value.Value;
    }
}

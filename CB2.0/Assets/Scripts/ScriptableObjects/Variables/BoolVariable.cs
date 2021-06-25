using UnityEngine;

[
    CreateAssetMenu(
        fileName = "BoolVar",
        menuName = "ScriptableObjects/Variables/BoolVar",
        order = 3)
]
public class BoolVariable : ScriptableObject
{
    private bool _value = false;

    public bool Value
    {
        get
        {
            return _value;
        }
    }

    public void Set(bool value)
    {
        _value = value;
    }

    public void Flip()
    {
        _value = !_value;
    }

    public void Set(BoolVariable value)
    {
        _value = value.Value;
    }
}

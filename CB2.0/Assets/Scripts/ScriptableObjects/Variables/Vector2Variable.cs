using UnityEngine;

[
    CreateAssetMenu(
        fileName = "Vector2",
        menuName = "ScriptableObjects/Variables/Vector2",
        order = 5)
]
public class Vector2Variable : ScriptableObject
{
    private Vector2 _value = Vector2.zero;

    public Vector2 Value
    {
        get
        {
            return _value;
        }
    }

    public void Set(Vector2 value)
    {
        _value = value;
    }

    public void Set(Vector2Variable value)
    {
        _value = value.Value;
    }

    public float GetX()
    {
        return _value.x;
    }

    public float GetY()
    {
        return _value.y;
    }

    public void SetX(float x)
    {
        _value.x = x;
    }

    public void SetY(float y)
    {
        _value.y = y;
    }
}

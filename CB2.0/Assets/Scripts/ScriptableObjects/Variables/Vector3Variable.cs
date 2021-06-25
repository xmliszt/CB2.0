using UnityEngine;

[
    CreateAssetMenu(
        fileName = "Vector3",
        menuName = "ScriptableObjects/Variables/Vector3",
        order = 6)
]
public class Vector3Variable : ScriptableObject
{
    private Vector3 _value = Vector3.zero;

    public Vector3 Value
    {
        get
        {
            return _value;
        }
    }

    public void Set(Vector3 value)
    {
        _value = value;
    }

    public void Set(Vector3Variable value)
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

    public float GetZ()
    {
        return _value.z;
    }

    public void SetX(float x)
    {
        _value.x = x;
    }

    public void SetY(float y)
    {
        _value.y = y;
    }

    public void SetZ(float z)
    {
        _value.z = z;
    }
}

using UnityEngine;

[
    CreateAssetMenu(
        fileName = "StringVar",
        menuName = "ScriptableObjects/StringVar",
        order = 4)
]
public class StringVariable : ScriptableObject
{
    private string _value = "";

    public string Value
    {
        get
        {
            return _value;
        }
    }

    public void Set(string value)
    {
        _value = value;
    }

    public void Set(StringVariable value)
    {
        _value = value.Value;
    }
}

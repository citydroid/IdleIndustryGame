using UnityEngine;

public class Increment
{
    private int _value;
    public SaveSystem _saveSystem;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            _saveSystem?.SaveGame();
        }
    }

    public Increment(int startValue = 0)
    {
        Value = startValue;
    }
}
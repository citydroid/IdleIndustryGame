using UnityEngine;

public class Result
{
    private MainScript _mainScript;
    private long _totalValue;
    public SaveSystem _saveSystem;
    public long TotalValue
    {
        get => _totalValue;
        set
        {
            _totalValue = System.Math.Max(0, value);
            _saveSystem?.SaveGame();
        }
    }

    public Result(MainScript mainScript, long startValue)
    {
        _mainScript = mainScript;
        _totalValue = startValue;
        _mainScript.StartCoroutine(AddIncrementCoroutine());
    }

    private System.Collections.IEnumerator AddIncrementCoroutine()
    {
        while (true)
        {
            TotalValue += _mainScript.increment.Value;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
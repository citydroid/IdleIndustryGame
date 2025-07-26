using UnityEngine;

public class Result
{
    private MainScript _mainScript;
    private int _totalValue;

    public int TotalValue
    {
        get => _totalValue;
        set => _totalValue = Mathf.Max(0, value); // не даём быть меньше 0
    }

    public Result(MainScript mainScript)
    {
        _mainScript = mainScript;
        _totalValue = 100;
        _mainScript.StartCoroutine(AddIncrementCoroutine());
    }

    private System.Collections.IEnumerator AddIncrementCoroutine()
    {
        while (true)
        {
            _totalValue += _mainScript.increment.Value;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
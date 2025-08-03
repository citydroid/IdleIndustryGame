using UnityEngine;

public class ResetButton : MonoBehaviour
{
    private SaveSystem _saveSystem;
    private MainScript _mainScript;

    private void Start()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        _mainScript = FindObjectOfType<MainScript>();

        if (_saveSystem == null || _mainScript == null)
        {
            Debug.LogError("SaveSystem or MainScript not found!");
        }
    }

    private void OnMouseDown()
    {
        if (_saveSystem != null && _mainScript != null)
        {
            _saveSystem.ResetGame();

            // Принудительно обновляем значения в MainScript
            _mainScript.result.TotalValue = _saveSystem.GetSavedTotalValue();
            _mainScript.increment.Value = _saveSystem.GetSavedIncrementValue();

            Debug.Log($"Game reset. New values: {_mainScript.result.TotalValue}, Increment: {_mainScript.increment.Value}");
        }
    }
}
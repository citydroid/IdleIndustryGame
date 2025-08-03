using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class SaveData
{
    public int totalValue = 1000000;
    public int incrementValue = 0;
}

public class SaveSystem : MonoBehaviour
{
    private SaveData _saveData = new SaveData();
    private string _savePath;

    public Result Result { get; set; }
    public Increment Increment { get; set; }

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame();

        Debug.Log($"SaveSystem initialized. Loaded values: TotalValue = {_saveData.totalValue}, Increment = {_saveData.incrementValue}");
        Debug.Log($"Save file path: {_savePath}");
    }

    public void InitializeBindings()
    {
        if (Increment != null)
        {
            Increment._saveSystem = this;
            Increment.Value = _saveData.incrementValue; // Применяем сохранённое значение
        }

        if (Result != null)
        {
            Result._saveSystem = this;
            Result.TotalValue = _saveData.totalValue; // Применяем сохранённое значение
        }
    }

    public void SaveGame()
    {
        try
        {
            if (Result != null) _saveData.totalValue = Result.TotalValue;
            if (Increment != null) _saveData.incrementValue = Increment.Value;

            string json = JsonUtility.ToJson(_saveData);
            File.WriteAllText(_savePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public void LoadGame()
    {
        try
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                _saveData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                _saveData = new SaveData();
                SaveGame();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            _saveData = new SaveData();
        }
    }

    public void ResetGame()
    {
        _saveData = new SaveData();
        SaveGame();

        if (Result != null) Result.TotalValue = 0;
        if (Increment != null) Increment.Value = 0;
    }

    public int GetSavedTotalValue() => _saveData.totalValue;
    public int GetSavedIncrementValue() => _saveData.incrementValue;
}

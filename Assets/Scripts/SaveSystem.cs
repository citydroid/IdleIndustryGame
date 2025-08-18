using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public long totalValue = 10000000000;
    public int incrementValue = 0;

}

public class SaveSystem : MonoBehaviour
{
    private SaveData _saveData = new SaveData();

    public Result Result { get; set; }
    public Increment Increment { get; set; }

    private const string SaveKey = "SaveData";

    private void Awake()
    {
        LoadGame();
        Debug.Log($"SaveSystem initialized. Loaded values: TotalValue = {_saveData.totalValue}, Increment = {_saveData.incrementValue}");

        // Ќ≈ вызываем InitializeBindings здесь Ч вызывай вручную после установки Result и Increment
    }

    public void InitializeBindings()
    {
        if (Increment != null)
        {
            Increment._saveSystem = this;
            Increment.Value = _saveData.incrementValue;
        }

        if (Result != null)
        {
            Result._saveSystem = this;
            Result.TotalValue = _saveData.totalValue;
        }
    }

    public void SaveGame()
    {
        try
        {
            if (Result != null) _saveData.totalValue = Result.TotalValue;
            if (Increment != null) _saveData.incrementValue = Increment.Value;

            string json = JsonUtility.ToJson(_saveData);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
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
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
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
        _saveData = new SaveData(); // сброс к начальному состо€нию
        SaveGame();

        if (Result != null) Result.TotalValue = _saveData.totalValue;
        if (Increment != null) Increment.Value = _saveData.incrementValue;
    }

    public long GetSavedTotalValue() => _saveData.totalValue;
    public int GetSavedIncrementValue() => _saveData.incrementValue;
}

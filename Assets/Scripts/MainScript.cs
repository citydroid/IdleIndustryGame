
using UnityEngine;
using TMPro;
using System.Globalization;

public class MainScript : MonoBehaviour
{
    public TextMeshPro resultText;
    public TextMeshPro incrementText;
    public TextMeshPro infoTextName;
    public TextMeshPro infoTextCost;
    public TextMeshPro infoTextCondition;

    public Increment increment;
    public Result result;
    public SaveSystem saveSystem;

    private void Start()
    {
        // Инициализируем SaveSystem первым
        saveSystem = new GameObject("SaveSystem").AddComponent<SaveSystem>();

        // Создаём объекты с загруженными значениями
        increment = new Increment(saveSystem.GetSavedIncrementValue());
        result = new Result(this, saveSystem.GetSavedTotalValue());

        // Сохраняем ссылки в SaveSystem
        saveSystem.Result = result;
        saveSystem.Increment = increment;
        saveSystem.InitializeBindings();

        InvokeRepeating(nameof(UpdateUI), 0, 0.1f);

    }

    private void UpdateUI()
    {
        string formattedResult = FormatCost(result.TotalValue);

        if (resultText != null)
            resultText.text = formattedResult;

        if (incrementText != null)
            incrementText.text = "+ " + increment.Value.ToString();
    }

    private string FormatCost(long cost)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", cost).Replace(",", ".");
    }

    public void ForceUpdateValues()
    {
        if (result != null && increment != null && saveSystem != null)
        {
            result.TotalValue = saveSystem.GetSavedTotalValue();
            increment.Value = saveSystem.GetSavedIncrementValue();
            UpdateUI(); // Принудительное обновление UI
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class IncrementChanger : MonoBehaviour
{
    public MainScript mainScript;
    public IncrementSettings incrementSettings;
    public int buttonIndex;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro incrementText;
    private TextMeshPro costText;
    private TextMeshPro nameText;
    private TextMeshPro levelText;
    private int currentCost;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ищем все TextMeshPro компоненты в дочерних объектах
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
        {
            if (text.name.Contains("IncrementText"))
                incrementText = text;
            else if (text.name.Contains("CostText"))
                costText = text;
            else if (text.name.Contains("NameText")) // Новое поле для имени
                nameText = text;
            else if (text.name.Contains("LevelText")) // Новое поле для уровня
                levelText = text;
        }

        CalculateCurrentCost();
        UpdateTexts();

        // Подписываемся на изменение локализации
        var data = incrementSettings.GetButtonData(buttonIndex);
        data.buttonName.StringChanged += UpdateNameText;
    }

    private void OnDestroy()
    {
        // Отписываемся при уничтожении объекта
        if (incrementSettings != null)
        {
            var data = incrementSettings.GetButtonData(buttonIndex);
            data.buttonName.StringChanged -= UpdateNameText;
        }
    }

    private void UpdateNameText(string localizedName)
    {
        if (nameText == null || incrementSettings == null) return;

        var data = incrementSettings.GetButtonData(buttonIndex);
        nameText.text = $"{localizedName}"; 
    }

    private void CalculateCurrentCost()
    {
        if (incrementSettings == null) return;
        var data = incrementSettings.GetButtonData(buttonIndex);
        currentCost = data.cost;
    }

    private void UpdateButtonState()
    {
        if (incrementSettings == null || mainScript == null) return;

        bool canAfford = mainScript.result.TotalValue >= currentCost;
        var data = incrementSettings.GetButtonData(buttonIndex);
        spriteRenderer.sprite = canAfford ? data.activeSprite : data.inactiveSprite;
    }

    private void UpdateLevelText()
    {
        if (levelText == null || incrementSettings == null) return;

        var data = incrementSettings.GetButtonData(buttonIndex);
        levelText.text = $"{data.level}";  
    }

    private void UpdateTexts()
    {
        if (incrementSettings == null) return;

        var data = incrementSettings.GetButtonData(buttonIndex);

        // Обновляем все текстовые поля
        if (incrementText != null)
            incrementText.text = "+" + data.incrementValue;

        if (costText != null)
            costText.text = currentCost.ToString();

        // Обновляем название (вызовет UpdateNameText)
        data.buttonName.RefreshString();

        // Явно обновляем уровень
        UpdateLevelText();
    }

    public void ForceUpdateTexts()
    {
        CalculateCurrentCost();
        UpdateTexts();
    }

    public void OnButtonClick()
    {
        if (mainScript.result.TotalValue < currentCost) return;

        var data = incrementSettings.GetButtonData(buttonIndex);

        // Основная логика
        mainScript.result.TotalValue -= currentCost;
        mainScript.increment.Value += data.incrementValue;

        // Увеличиваем стоимость
        data.cost += data.costCoefficient;
        data.costCoefficient = Mathf.RoundToInt((data.costCoefficient * 0.5f) + data.costCoefficient);

        // Увеличиваем уровень
        data.level++;

        // Обновляем только уровень (название не меняется)
        UpdateLevelText();

        // Обновляем остальные тексты
        CalculateCurrentCost();
        if (costText != null)
            costText.text = currentCost.ToString();
    }
}
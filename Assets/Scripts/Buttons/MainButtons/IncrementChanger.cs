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

        // ���� ��� TextMeshPro ���������� � �������� ��������
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
        {
            if (text.name.Contains("IncrementText"))
                incrementText = text;
            else if (text.name.Contains("CostText"))
                costText = text;
            else if (text.name.Contains("NameText")) // ����� ���� ��� �����
                nameText = text;
            else if (text.name.Contains("LevelText")) // ����� ���� ��� ������
                levelText = text;
        }

        CalculateCurrentCost();
        UpdateTexts();

        // ������������� �� ��������� �����������
        var data = incrementSettings.GetButtonData(buttonIndex);
        data.buttonName.StringChanged += UpdateNameText;
    }

    private void OnDestroy()
    {
        // ������������ ��� ����������� �������
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

        // ��������� ��� ��������� ����
        if (incrementText != null)
            incrementText.text = "+" + data.incrementValue;

        if (costText != null)
            costText.text = currentCost.ToString();

        // ��������� �������� (������� UpdateNameText)
        data.buttonName.RefreshString();

        // ���� ��������� �������
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

        // �������� ������
        mainScript.result.TotalValue -= currentCost;
        mainScript.increment.Value += data.incrementValue;

        // ����������� ���������
        data.cost += data.costCoefficient;
        data.costCoefficient = Mathf.RoundToInt((data.costCoefficient * 0.5f) + data.costCoefficient);

        // ����������� �������
        data.level++;

        // ��������� ������ ������� (�������� �� ��������)
        UpdateLevelText();

        // ��������� ��������� ������
        CalculateCurrentCost();
        if (costText != null)
            costText.text = currentCost.ToString();
    }
}
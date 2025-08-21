using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System.Globalization;
using System;

public class IncrementChanger : MonoBehaviour
{
    public MainScript mainScript;

    [System.Serializable]
    public class ButtonData
    {
        public int incrementValue;
        public int incrementCoefficient;
        public LocalizedString buttonName;
        public int level = 1;

        public long cost;
        public long costCoefficient;
    }

    [Header("Button Settings")]
    [SerializeField] private ButtonData buttonData;
    [SerializeField] public int buttonIndex;

    private IncrementButton incrementButton;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro incrementText;
    private TextMeshPro costText;
    private TextMeshPro nameText;
    private TextMeshPro levelText;

    private long currentCost;
    private bool isPointerOver = false;

    public Color priceTextColor = new Color(0.67f, 0.67f, 0.67f); // #AAAAAA
    public Color priceValueColor = Color.white;
    public Color nameTextColor = Color.yellow;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        incrementButton = GetComponent<IncrementButton>();

        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
        {
            if (text.name.Contains("IncrementText")) incrementText = text;
            else if (text.name.Contains("CostText")) costText = text;
            else if (text.name.Contains("NameText")) nameText = text;
            else if (text.name.Contains("LevelText")) levelText = text;
        }

        CalculateCurrentCost();
        UpdateTexts();

        if (buttonData.buttonName != null)
            buttonData.buttonName.StringChanged += UpdateNameText;
    }

    private void OnDestroy()
    {
        if (buttonData.buttonName != null)
            buttonData.buttonName.StringChanged -= UpdateNameText;
    }

    private void Update()
    {
        UpdateButtonState();
        if (isPointerOver)
            UpdateInfoText();
    }

    public void OnPointerEnter()
    {
        isPointerOver = true;
        UpdateInfoText();
    }

    public void OnPointerExit()
    {
        isPointerOver = false;

        if (mainScript != null)
        {
            mainScript.infoTextName.text = "";
            mainScript.infoTextCost.text = "";
            mainScript.infoTextCondition.text = "";
        }
    }

    public void OnPointerDown()
    {
        if (!CanAfford())
            UpdateInfoText();
    }

    private void UpdateInfoText()
    {
        if (mainScript == null) return;

        if (mainScript.infoTextName != null)
            mainScript.infoTextName.text = $"<color=#{ColorUtility.ToHtmlStringRGB(nameTextColor)}>{buttonData.buttonName.GetLocalizedString()} {buttonData.level + 1}</color>";

        if (mainScript.infoTextCost != null)
        {
            string formattedCost = FormatCost(currentCost);
            mainScript.infoTextCost.text =
                $"<color=#{ColorUtility.ToHtmlStringRGB(priceTextColor)}>����:</color> " +
                $"<color=#{ColorUtility.ToHtmlStringRGB(priceValueColor)}>{formattedCost}</color>";
        }

        if (mainScript.infoTextCondition != null)
            mainScript.infoTextCondition.text = $"+{buttonData.incrementValue} � �������";
    }

    private string FormatCost(long cost)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", cost).Replace(",", ".");
    }

    public bool CanAffordPublic() => CanAfford();

    private bool CanAfford()
    {
        return mainScript != null && mainScript.result != null &&
               mainScript.result.TotalValue >= currentCost;
    }

    private void UpdateNameText(string localizedName)
    {
        if (nameText == null) return;
        nameText.text = localizedName;
    }

    private void CalculateCurrentCost()
    {
        currentCost = buttonData.cost + buttonData.costCoefficient;
    }

    private void UpdateButtonState()
    {
        bool canAfford = CanAfford();
        if (incrementButton != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = canAfford ? incrementButton.activeSprite : incrementButton.inactiveSprite;

            if (!canAfford)
                incrementButton.ResetScale();
        }
    }

    private void UpdateLevelText()
    {
        if (levelText == null) return;
        levelText.text = buttonData.level == 0 ? ">" : buttonData.level.ToString();
    }

    private void UpdateTexts()
    {
        if (incrementText != null)
            incrementText.text = "+" + buttonData.incrementValue;

        if (costText != null)
            costText.text = FormatCost(currentCost);

        buttonData.buttonName?.RefreshString();
        UpdateLevelText();
    }

    public void ForceUpdateTexts()
    {
        CalculateCurrentCost();
        UpdateTexts();
    }

    public long GetCurrentCost()
    {
        return currentCost;
    }

    public void AddIncrementValue(int value)
    {
        buttonData.incrementValue += value;
    }

    public int GetIncrementValue()
    {
        return buttonData.incrementValue;
    }

    public void SetIncrementValue(int value)
    {
        buttonData.incrementValue = value;
    }

    public void OnButtonClick()
    {
        if (!CanAfford())
        {
            UpdateInfoText();
            return;
        }

        mainScript.result.TotalValue -= currentCost;
        mainScript.increment.Value += buttonData.incrementValue;

        buttonData.cost += buttonData.costCoefficient;
        buttonData.costCoefficient = (long)Math.Round(buttonData.costCoefficient * 1.5);
        buttonData.level++;

        UpdateLevelText();
        CalculateCurrentCost();

        if (costText != null)
            costText.text = FormatCost(currentCost);
    }
}

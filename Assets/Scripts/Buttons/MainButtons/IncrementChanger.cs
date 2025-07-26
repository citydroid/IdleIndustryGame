using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System.Globalization;

public class IncrementChanger : MonoBehaviour
{
    public MainScript mainScript;
    public IncrementSettings incrementSettings;
    private IncrementButton incrementButton;
    public int buttonIndex;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro incrementText;
    private TextMeshPro costText;
    private TextMeshPro nameText;
    private TextMeshPro levelText;
    private int currentCost;
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
            if (text.name.Contains("IncrementText"))
                incrementText = text;
            else if (text.name.Contains("CostText"))
                costText = text;
            else if (text.name.Contains("NameText"))
                nameText = text;
            else if (text.name.Contains("LevelText"))
                levelText = text;
        }

        CalculateCurrentCost();
        UpdateTexts();

        var data = incrementSettings.GetButtonData(buttonIndex);
        data.buttonName.StringChanged += UpdateNameText;
    }

    private void Update()
    {
        UpdateButtonState();

        if (isPointerOver)
        {
            UpdateInfoText();
        }
    }

    private void OnDestroy()
    {
        if (incrementSettings != null)
        {
            var data = incrementSettings.GetButtonData(buttonIndex);
            data.buttonName.StringChanged -= UpdateNameText;
        }
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
        {
            UpdateInfoText();
        }
    }

    private void UpdateInfoText()
    {
        if (mainScript == null || incrementSettings == null) return;

        var data = incrementSettings.GetButtonData(buttonIndex);

        if (mainScript.infoTextName != null)
            mainScript.infoTextName.text = $"<color=#{ColorUtility.ToHtmlStringRGB(nameTextColor)}>{data.buttonName.GetLocalizedString()} {data.level + 1}</color>";

    if (mainScript.infoTextCost != null)
    {
        string formattedCost = FormatCost(currentCost);
        mainScript.infoTextCost.text = 
            $"<color=#{ColorUtility.ToHtmlStringRGB(priceTextColor)}>Цена:</color> " +
            $"<color=#{ColorUtility.ToHtmlStringRGB(priceValueColor)}>{formattedCost}</color>";
    }

        if (mainScript.infoTextCondition != null)
            mainScript.infoTextCondition.text = $"+{data.incrementValue} в секунду";
    }

    private string FormatCost(int cost)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", cost).Replace(",", ".");
    }
    public bool CanAffordPublic()
    {
        return CanAfford();
    }
    private bool CanAfford()
    {
        return mainScript != null && mainScript.result != null &&
               mainScript.result.TotalValue >= currentCost;
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
        currentCost = data.cost + data.costCoefficient;
    }

    private void UpdateButtonState()
    {
        if (incrementSettings == null || mainScript == null) return;

        bool canAfford = CanAfford();
        var data = incrementSettings.GetButtonData(buttonIndex);

        if (incrementButton != null)
        {
            spriteRenderer.sprite = canAfford ? incrementButton.activeSprite : incrementButton.inactiveSprite;
        }

        if (!canAfford && incrementButton != null)
        {
            incrementButton.ResetScale();
        }
    }


    private void UpdateLevelText()
    {
        if (levelText == null || incrementSettings == null) return;

        var data = incrementSettings.GetButtonData(buttonIndex);

        if (data.level == 0)
        {
            levelText.text = ">";
        }
        else
        {
            levelText.text = data.level.ToString();
        }
    }

    private void UpdateTexts()
    {
        if (incrementSettings == null) return;

        var data = incrementSettings.GetButtonData(buttonIndex);

        if (incrementText != null)
            incrementText.text = "+" + data.incrementValue;

        if (costText != null)
            costText.text = FormatCost(currentCost); 

        data.buttonName.RefreshString();
        UpdateLevelText();
    }

    public void ForceUpdateTexts()
    {
        CalculateCurrentCost();
        UpdateTexts();
    }

    public void OnButtonClick()
    {
        if (!CanAfford())
        {
            UpdateInfoText();
            return;
        }

        var data = incrementSettings.GetButtonData(buttonIndex);

        // Основная логика
        mainScript.result.TotalValue -= currentCost;
        mainScript.increment.Value += data.incrementValue;

        // Увеличиваем стоимость
        data.cost += data.costCoefficient;
        data.costCoefficient = Mathf.RoundToInt((data.costCoefficient * 0.5f) + data.costCoefficient);

        data.level++;

        UpdateLevelText();
        CalculateCurrentCost();

        if (costText != null)
            costText.text = FormatCost(currentCost); 
    }
}
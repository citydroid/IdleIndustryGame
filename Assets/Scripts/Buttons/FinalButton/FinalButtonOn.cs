using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System.Collections.Generic;
using System.Globalization;

public class FinalButtonOn : MonoBehaviour
{
    public MainScript mainScript; 
    
    [System.Serializable]
    public class ButtonData
    {
        public LocalizedString buttonName;
    }

    [Header("Button Settings")]
    [SerializeField] private ButtonData buttonData;

    [Header("Activation Settings")]
    [SerializeField] private int activationValue = 7; 
    private int currentValue = 0; 

    private SpriteRenderer spriteRenderer;
    private FinalButtonOnUI buttonUI;

    private TextMeshPro incrementText;
    private TextMeshPro costText;
    private TextMeshPro nameText;

    private bool isActivated = false;
    private bool isPointerOver = false;

    [Header("Button Actions")]
    [SerializeField] private List<MonoBehaviour> actionComponents = new List<MonoBehaviour>(); 
    private List<IButtonAction> actions = new List<IButtonAction>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buttonUI = GetComponent<FinalButtonOnUI>();

        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
        {
            if (text.name.Contains("IncrementText")) incrementText = text;
            else if (text.name.Contains("CostText")) costText = text;
            else if (text.name.Contains("NameText")) nameText = text;
        }

        if (buttonData.buttonName != null)
            buttonData.buttonName.StringChanged += UpdateNameText;

        foreach (var component in actionComponents)
        {
            if (component is IButtonAction action)
                actions.Add(action);
        }

        UpdateTexts();
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        if (buttonData.buttonName != null)
            buttonData.buttonName.StringChanged -= UpdateNameText;
    }

    private void Update()
    {
        UpdateButtonState();
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
            UpdateInfoText();
    }
    private void UpdateInfoText()
    {
        if (mainScript == null) return;

        // Name text (Level Up for next level)
        if (mainScript.infoTextName != null)
            mainScript.infoTextName.text = $"<color=yellow>{buttonData.buttonName.GetLocalizedString()}</color>";

        // Cost text
        if (mainScript.infoTextCost != null)
        {
            string priceLabelColor = "#00FF00"; 

            mainScript.infoTextCost.text =
                $"<color={priceLabelColor}>{TextStandart.GetPriceLabel()}</color> " + 
                $"<color=white> {TextStandart.GetFreeLabel()} </color>";
        }

        /*
        if (mainScript.infoTextCondition != null)
            mainScript.infoTextCondition.text = $"+{TextStandart.GetPriceLabel()} в секунду"; 
        */
    }
    private void UpdateButtonState()
    {
        bool canActivate = currentValue >= activationValue;

        if (buttonUI != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = canActivate ? buttonUI.activeSprite : buttonUI.inactiveSprite;

            if (!canActivate)
                buttonUI.ResetScale();
        }

        isActivated = canActivate;
    }

    private void UpdateNameText(string localizedName)
    {
        if (nameText == null) return;
        nameText.text = localizedName;
    }

    private void UpdateTexts()
    {
        if (incrementText != null)
            incrementText.text = currentValue.ToString();

        buttonData.buttonName?.RefreshString();
    }

    public void SetCurrentValue(int value)
    {
        currentValue = currentValue + value;
        UpdateTexts();
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    // Если нужно что-то выполнить при нажатии
    public void OnButtonClick()
    {
        if (!isActivated) return;

        Debug.Log($"{buttonData.buttonName.GetLocalizedString()} кнопка активирована! {currentValue}");


        foreach (var action in actions)
        {
            action.Execute();
        }

        SetCurrentValue(0);


        // Пауза
        FindObjectOfType<MusicManager>().PauseMusic();

        // Возобновление
        FindObjectOfType<MusicManager>().ResumeMusic();

        // Мгновенно выключить
        FindObjectOfType<MusicManager>().StopMusic();

        // Плавное выключение с затуханием
        FindObjectOfType<MusicFader>().FadeOutAndStop();
    }
}

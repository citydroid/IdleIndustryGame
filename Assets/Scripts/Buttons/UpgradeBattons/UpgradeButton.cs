using UnityEngine;
using UnityEngine.Localization;
using System.Globalization;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class UpgradeButton : MonoBehaviour
{
    public enum UnlockConditionType { CostCheck, ExternalTrigger, Hybrid }

    [System.Serializable]
    public class ButtonData
    {
        public int incrementValue;
        public int targetButtonIndex;
    }

    [Header("Main Settings")]
    [SerializeField] private MainScript mainScript;
    [SerializeField] private IncrementSettings incrementSettings;

    [Header("Upgrade Settings")]
    [SerializeField] private ButtonData buttonData;
    [SerializeField] private int cost;

    [Header("Unlock Settings")]
    [SerializeField] private UnlockConditionType unlockCondition = UnlockConditionType.CostCheck;
    [SerializeField] private int checkButtonIndex;
    [SerializeField] private int targetCostToUnlock;
    [SerializeField] public bool requireOtherButtonActivation = false;
    [SerializeField] public UpgradeButton requiredButton;

    [Header("Localization")]
    [SerializeField] private LocalizedString buttonName;
    [SerializeField] public LocalizedString conditionText;

    [HideInInspector] public bool purchased = false;

    private bool externalTriggerFlag;
    private bool conditionsMet;

    public bool IsUnlocked => conditionsMet;
    public bool IsAffordable => mainScript != null && mainScript.result.TotalValue >= cost;
    public bool CanPurchase => IsAffordable && conditionsMet && !purchased;

    public MainScript Main => mainScript;
    public int Cost => cost;
    public string RawButtonName => gameObject.name;

    private void Awake()
    {
        if (buttonData == null)
        {
            Debug.LogError("ButtonData is not assigned!");
            enabled = false;
        }
    }

    private void Start()
    {
        if (mainScript == null)
            mainScript = FindObjectOfType<MainScript>();

        if (incrementSettings == null)
            incrementSettings = FindObjectOfType<IncrementSettings>();

        CheckUnlockConditions();
    }

    private void Update()
    {
        if (!purchased)
        {
            CheckUnlockConditions();
        }
    }

    public void TryPurchase()
    {
        if (!CanPurchase || buttonData == null)
            return;

        mainScript.result.TotalValue -= cost;

        var targetData = incrementSettings.GetButtonData(buttonData.targetButtonIndex);
        if (targetData != null)
        {
            targetData.incrementValue += buttonData.incrementValue;

            foreach (var changer in FindObjectsOfType<IncrementChanger>())
            {
                if (changer.buttonIndex == buttonData.targetButtonIndex)
                {
                    changer.ForceUpdateTexts();
                }
            }
        }

        purchased = true;
    }

    private void CheckUnlockConditions()
    {
        if (conditionsMet) return;

        if (requireOtherButtonActivation && requiredButton != null && !requiredButton.purchased)
            return;

        switch (unlockCondition)
        {
            case UnlockConditionType.CostCheck:
                conditionsMet = CheckCostCondition();
                break;
            case UnlockConditionType.ExternalTrigger:
                conditionsMet = externalTriggerFlag;
                break;
            case UnlockConditionType.Hybrid:
                conditionsMet = externalTriggerFlag && CheckCostCondition();
                break;
        }
    }

    private bool CheckCostCondition()
    {
        var incrementData = incrementSettings.GetButtonData(checkButtonIndex);
        return incrementData != null && incrementData.cost >= targetCostToUnlock;
    }

    public void SetExternalTrigger()
    {
        if (unlockCondition == UnlockConditionType.ExternalTrigger || unlockCondition == UnlockConditionType.Hybrid)
            externalTriggerFlag = true;
    }

    public string GetUnlockConditionText()
    {
        if (purchased)
            return "Уже куплено";

        // Если есть кастомный текст, используем его
        if (!string.IsNullOrEmpty(conditionText.GetLocalizedString()))
            return conditionText.GetLocalizedString();

        // Проверяем требование другой кнопки
        if (requireOtherButtonActivation && requiredButton != null && !requiredButton.purchased)
            return $"Требуется: {requiredButton.GetLocalizedButtonName()}";

        // Для первой кнопки (без требований)
        if (!requireOtherButtonActivation && requiredButton == null &&
            unlockCondition != UnlockConditionType.ExternalTrigger)
            return "Доступно для покупки";

        // Остальные случаи
        switch (unlockCondition)
        {
            case UnlockConditionType.CostCheck:
                var incrementData = incrementSettings.GetButtonData(checkButtonIndex);
                int currentCost = incrementData?.cost ?? 0;
                return $"Требуется: кнопка {checkButtonIndex} (${targetCostToUnlock}) [{currentCost}/{targetCostToUnlock}]";

            case UnlockConditionType.ExternalTrigger:
                return "Требуется внешнее событие";

            case UnlockConditionType.Hybrid:
                return $"Требуется: внешнее событие + кнопка {checkButtonIndex} (${targetCostToUnlock})";

            default:
                return "Доступно для покупки";
        }
    }

    public string FormatCost()
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", cost).Replace(",", ".");
    }

    public string GetLocalizedButtonName()
    {
        return buttonName != null ? buttonName.GetLocalizedString() : RawButtonName;
    }
    public void ForceCheckConditions()
    {
        CheckUnlockConditions();
    }
}

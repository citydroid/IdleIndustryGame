using UnityEngine;
using UnityEngine.Localization;
using System.Globalization;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class UpgradeButton : MonoBehaviour
{
    public enum UnlockConditionType { CostCheck, ExternalTrigger, Hybrid }

    [System.Serializable]
    public class ButtonData
    {
        public int incrementValue;
    }

    [Header("Main Settings")]
    [SerializeField] private MainScript mainScript;
    private SaveSystem _saveSystem;

    [Header("Upgrade Settings")]
    [SerializeField] private ButtonData buttonData;
    [SerializeField] private int cost;
    [SerializeField] private IncrementChanger targetChanger;

    [Header("Unlock Settings")]
    [SerializeField] private UnlockConditionType unlockCondition = UnlockConditionType.CostCheck;
    [SerializeField] private IncrementChanger changerToCheck;
    [SerializeField] private int targetCostToUnlock;
    [SerializeField] public bool requireOtherButtonActivation = false;
    [SerializeField] public UpgradeButton requiredButton;

    [Header("Action on Purchase")]
    [SerializeField] private List<MonoBehaviour> buttonActions = new List<MonoBehaviour>(); // Список скриптов, реализующих IButtonAction
    private List<IButtonAction> _cachedActions = new List<IButtonAction>();

    [Header("Localization")]
    [SerializeField] private LocalizedString buttonName;
    [SerializeField] public LocalizedString conditionText;

    [HideInInspector] public bool purchased = false;

    private bool externalTriggerFlag;
    private bool conditionsMet;

    public bool IsUnlocked => conditionsMet;
    public bool IsAffordable
    {
        get
        {
            if (mainScript != null && mainScript.result != null)
                return mainScript.result.TotalValue >= cost;

            if (_saveSystem == null)
                _saveSystem = FindObjectOfType<SaveSystem>();

            if (_saveSystem != null)
                return _saveSystem.GetSavedTotalValue() >= cost;

            return new SaveData().totalValue >= cost;
        }
    }
    public bool CanPurchase => IsAffordable && conditionsMet && !purchased;

    public MainScript Main => mainScript;
    public int Cost => cost;
    public string RawButtonName => gameObject.name;

    private void Awake()
    {
        if (buttonData == null || targetChanger == null)
        {
            Debug.LogError("UpgradeButton: buttonData или targetChanger не назначен!");
            enabled = false;
        }

        // Кэшируем все действительные IButtonAction из списка
        foreach (var action in buttonActions)
        {
            if (action != null && action is IButtonAction buttonAction)
            {
                _cachedActions.Add(buttonAction);
            }
            else if (action != null)
            {
                Debug.LogWarning($"{action.name} не реализует IButtonAction.");
            }
        }
    }

    private void Start()
    {
        InitializeReferences();
        CheckUnlockConditions();
    }

    private void InitializeReferences()
    {
        if (mainScript == null)
            mainScript = FindObjectOfType<MainScript>();

        if (_saveSystem == null)
            _saveSystem = FindObjectOfType<SaveSystem>();
    }

    private void Update()
    {
        if (!purchased)
            CheckUnlockConditions();
    }

    public void TryPurchase()
    {
        if (!CanPurchase)
            return;

        if (mainScript != null && mainScript.result != null)
        {
            mainScript.result.TotalValue -= cost;
        }
        else if (_saveSystem != null)
        {
            int newValue = _saveSystem.GetSavedTotalValue() - cost;
            _saveSystem.Result.TotalValue = newValue;
            _saveSystem.SaveGame();
        }

        targetChanger?.AddIncrementValue(buttonData.incrementValue);
        targetChanger?.ForceUpdateTexts();

        // Выполняем все зарегистрированные действия
        foreach (var action in _cachedActions)
        {
            action?.Execute();
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
        return changerToCheck != null && changerToCheck.GetCurrentCost() >= targetCostToUnlock;
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

        if (!string.IsNullOrEmpty(conditionText.GetLocalizedString()))
            return conditionText.GetLocalizedString();

        if (requireOtherButtonActivation && requiredButton != null && !requiredButton.purchased)
            return $"Требуется: {requiredButton.GetLocalizedButtonName()}";

        if (!requireOtherButtonActivation && requiredButton == null &&
            unlockCondition != UnlockConditionType.ExternalTrigger)
            return "Доступно для покупки";

        switch (unlockCondition)
        {
            case UnlockConditionType.CostCheck:
                int currentCost = changerToCheck?.GetCurrentCost() ?? 0;
                return $"Требуется: стоимость ≥ {targetCostToUnlock} [{currentCost}/{targetCostToUnlock}]";

            case UnlockConditionType.ExternalTrigger:
                return "Требуется внешнее событие";

            case UnlockConditionType.Hybrid:
                return $"Требуется: внешнее событие + стоимость ≥ {targetCostToUnlock}";

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
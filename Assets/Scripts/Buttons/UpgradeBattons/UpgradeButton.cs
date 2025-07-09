using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class UpgradeButton : MonoBehaviour
{
    public enum UnlockConditionType
    {
        CostCheck,
        ExternalTrigger,
        Hybrid
    }

    [System.Serializable]
    public class ButtonData
    {
        public int incrementValue;
        public int targetButtonIndex;
        public Sprite activeSprite;
        public Sprite inactiveSprite;
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
    [SerializeField] private bool requireOtherButtonActivation = false;
    [SerializeField] private UpgradeButton requiredButton;


    private SpriteRenderer buttonRenderer;
    private Collider2D buttonCollider;
    private bool purchased = false;
    private bool externalTriggerFlag;
    private bool conditionsMet;

    private void Awake()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        buttonCollider = GetComponent<Collider2D>();

        if (buttonData == null)
        {
            Debug.LogError("ButtonData is not assigned!");
            enabled = false;
        }
    }

    private void Start()
    {
        if (mainScript == null)
        {
            mainScript = FindObjectOfType<MainScript>();
            if (mainScript == null)
            {
                Debug.LogError("MainScript not found in scene!");
                return;
            }
        }

        if (mainScript.result == null)
        {
            Debug.LogError("MainScript.result is null!");
            return;
        }

        if (incrementSettings == null)
        {
            incrementSettings = FindObjectOfType<IncrementSettings>();
            if (incrementSettings == null)
            {
                Debug.LogError("IncrementSettings not found!");
                return;
            }
        }

        UpdateButtonState();
    }

    private void Update()
    {
        if (purchased) return;

        CheckUnlockConditions();
        UpdateButtonState();
        HandleInput();
    }

    private void CheckUnlockConditions()
    {
        if (conditionsMet) return;

        if (requireOtherButtonActivation && requiredButton != null)
        {
            if (!requiredButton.HasBeenPurchased())
            {
                return;
            }
        }

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
    public bool HasBeenPurchased()
    {
        return purchased;
    }
    private bool CheckCostCondition()
    {
        if (incrementSettings == null) return false;
        var incrementData = incrementSettings.GetButtonData(checkButtonIndex);
        return incrementData != null && incrementData.cost >= targetCostToUnlock;
    }

    private void UpdateButtonState()
    {
        bool canAfford = mainScript.result.TotalValue >= cost;
        bool shouldBeActive = canAfford && conditionsMet && !purchased;

        buttonCollider.enabled = shouldBeActive;
        if (buttonData != null)
        {
            buttonRenderer.sprite = shouldBeActive ? buttonData.activeSprite : buttonData.inactiveSprite;
        }
    }

    private void HandleInput()
    {
        if (!buttonCollider.enabled || purchased) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider == buttonCollider)
            {
                TryPurchaseUpgrade();
            }
        }
    }

    private void TryPurchaseUpgrade()
    {
        if (mainScript.result.TotalValue >= cost && buttonData != null)
        {
            mainScript.result.TotalValue -= cost;

            var targetData = incrementSettings.GetButtonData(buttonData.targetButtonIndex);
            if (targetData != null)
            {
                targetData.incrementValue += buttonData.incrementValue;

                var changers = FindObjectsOfType<IncrementChanger>();
                foreach (var changer in changers)
                {
                    if (changer.buttonIndex == buttonData.targetButtonIndex)
                    {
                        changer.ForceUpdateTexts();
                    }
                }

            }

            purchased = true;
            buttonCollider.enabled = false;
            UpdateButtonState();

            Debug.Log($"Upgrade purchased: +{buttonData.incrementValue} to button {buttonData.targetButtonIndex}");
        }
    }

    public void SetExternalTrigger()
    {
        if (unlockCondition == UnlockConditionType.ExternalTrigger ||
            unlockCondition == UnlockConditionType.Hybrid)
        {
            externalTriggerFlag = true;
        }
    }

    private void OnMouseEnter()
    {
        if (buttonCollider.enabled)
            transform.localScale = Vector3.one * 1.1f;
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
    }
}
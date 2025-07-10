using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(UpgradeButton))]
public class UpgradeButtonUI : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Sprites")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public Sprite closedSprite;

    [Header("UI Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;
    public Color disabledColor = Color.black;

    private SpriteRenderer spriteRenderer;
    private UpgradeButton logic;
    private bool isPointerOver;
    private Vector3 originalScale;
    private bool forceShowInfo = false;
    private float infoShowTime = 2f;
    private float infoShowTimer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        logic = GetComponent<UpgradeButton>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        UpdateVisualState();
    }

    private void Update()
    {
        UpdateVisualState();

        if (forceShowInfo)
        {
            infoShowTimer -= Time.deltaTime;
            if (infoShowTimer <= 0f)
            {
                forceShowInfo = false;
                if (!isPointerOver)
                {
                    ClearInfo();
                }
            }
        }
    }

    private void UpdateVisualState()
    {
        if (logic == null) return;

        if (logic.purchased)
        {
            spriteRenderer.sprite = closedSprite;
            spriteRenderer.color = disabledColor;
        }
        else if (!logic.IsUnlocked || !logic.IsAffordable)
        {
            spriteRenderer.sprite = inactiveSprite;
            spriteRenderer.color = disabledColor;
        }
        else
        {
            spriteRenderer.sprite = activeSprite;
            spriteRenderer.color = isPointerOver ? hoverColor : normalColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!logic.purchased && logic.IsUnlocked && logic.IsAffordable)
        {
            transform.localScale = originalScale * 0.9f;
        }

        if (logic.CanPurchase)
        {
            logic.TryPurchase();
        }
        else
        {
            forceShowInfo = true;
            infoShowTimer = infoShowTime;
        }

        ShowInfo();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        ShowInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        if (!forceShowInfo)
        {
            ClearInfo();
        }
    }

    private void ShowInfo()
    {
        if (logic.Main == null) return;

        // Всегда показываем название
        if (logic.Main.infoTextName != null)
        {
            logic.Main.infoTextName.text = $"{logic.GetLocalizedButtonName()}";
        }

        // Всегда показываем цену
        if (logic.Main.infoTextCost != null)
        {
            logic.Main.infoTextCost.text = $"<color=yellow>Цена:</color> <color=white>{logic.FormatCost()}</color>";
        }

        // Условия разблокировки
        if (logic.Main.infoTextCondition != null)
        {
            logic.ForceCheckConditions();
            string conditionText = logic.GetUnlockConditionText();

            // Если есть кастомный текст условия, используем его
            if (!string.IsNullOrEmpty(logic.conditionText.GetLocalizedString()))
            {
                conditionText = logic.conditionText.GetLocalizedString();
            }
            // Иначе проверяем стандартные условия
            else if (logic.requireOtherButtonActivation &&
                    logic.requiredButton != null &&
                    !logic.requiredButton.purchased)
            {
                conditionText = $"Требуется: {logic.requiredButton.GetLocalizedButtonName()}";
            }

            logic.Main.infoTextCondition.text = $"{conditionText}";
        }
    }

    private void ClearInfo()
    {
        if (logic.Main == null) return;

        if (logic.Main.infoTextName != null)
            logic.Main.infoTextName.text = "";

        if (logic.Main.infoTextCost != null)
            logic.Main.infoTextCost.text = "";

        if (logic.Main.infoTextCondition != null)
            logic.Main.infoTextCondition.text = "";
    }
}
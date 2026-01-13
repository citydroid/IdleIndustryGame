using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(UpgradeButton))]
[RequireComponent(typeof(AudioSource))]
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

    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.5f;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;

        audioSource.playOnAwake = false;
        audioSource.volume = soundVolume;
        if (clickSound != null)
        {
            audioSource.clip = clickSound;
        }
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
            if (clickSound != null)
            {
                audioSource.Play();
            }
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

        if (logic.Main.infoTextName != null)
        {
            logic.Main.infoTextName.text = $"<color=yellow>{logic.GetLocalizedButtonName()}</color>";
        }

        if (logic.Main.infoTextCost != null)
        {
            string priceLabelColor = logic.IsAffordable ? "#00FF00" : "#FF0000";

            logic.Main.infoTextCost.text =
                $"<color={priceLabelColor}>{TextStandart.GetPriceLabel()}</color> <color=white>{logic.FormatCost()}</color>";
        }

        // Условия разблокировки
        if (logic.Main.infoTextCondition != null)
        {
            logic.ForceCheckConditions();
           // string conditionText = logic.GetUnlockConditionText();

            string conditionTextColor = logic.IsUnlocked ? "#00FF00" : "#FF0000";

            // Если есть кастомный текст условия, используем его
 /*            if (!string.IsNullOrEmpty(logic.conditionText.GetLocalizedString()))
            {
                conditionText = $"{TextStandart.GetRequiresLabel()} {logic.conditionText.GetLocalizedString()}";
            }
           else if (logic.requireOtherButtonActivation &&
                     logic.requiredButton != null &&
                     !logic.requiredButton.purchased)
            {
                conditionText = $"{TextStandart.GetRequiresLabel()} {logic.requiredButton.GetLocalizedButtonName()}";
                conditionTextColor = "#FF0000";
            }
            else if (!logic.IsUnlocked)
            {
                conditionTextColor = "#FF0000";
            }
            else
            {
                conditionTextColor = "#00FF00";
            }*/
            Debug.Log($"GetRequiresLabel() = '{TextStandart.GetRequiresLabel()}'");
            logic.Main.infoTextCondition.text = $"<color={conditionTextColor}>{TextStandart.GetRequiresLabel()}</color> <color=white>{logic.conditionText.GetLocalizedString()}</color>";
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

    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = soundVolume;
        }
    }
}
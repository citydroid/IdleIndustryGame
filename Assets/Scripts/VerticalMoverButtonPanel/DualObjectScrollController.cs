using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SmoothScrollingController))]
public class DualObjectScrollController : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    public GameObject scrollUpObject;
    public GameObject scrollDownObject;

    [Header("Ограничения уровней")]
    public int minLevel = 0;
    public int maxLevel = 2;

    [Header("Спрайты")]
    public Sprite activeUpSprite;
    public Sprite inactiveUpSprite;
    public Sprite activeDownSprite;
    public Sprite inactiveDownSprite;

    private SpriteRenderer upRenderer;
    private SpriteRenderer downRenderer;

    private SmoothScrollingController scrollingController;
    private int currentLevel = 0;

    private Vector3 upOriginalScale;
    private Vector3 downOriginalScale;

    void Awake()
    {
        scrollingController = GetComponent<SmoothScrollingController>();

        if (!scrollingController)
        {
            Debug.LogError("Не найден SmoothScrollingController на этом объекте!");
            return;
        }

        if (scrollUpObject)
        {
            upRenderer = scrollUpObject.GetComponent<SpriteRenderer>();
            upOriginalScale = scrollUpObject.transform.localScale;
            AddEventTriggers(scrollUpObject, ScrollUp);
        }

        if (scrollDownObject)
        {
            downRenderer = scrollDownObject.GetComponent<SpriteRenderer>();
            downOriginalScale = scrollDownObject.transform.localScale;
            AddEventTriggers(scrollDownObject, ScrollDown);
        }

        scrollingController.ScrollToLevel(currentLevel);
        UpdateVisualState();
    }

    void UpdateVisualState()
    {
        if (upRenderer)
        {
            bool upActive = currentLevel > minLevel;
            upRenderer.sprite = upActive ? activeUpSprite : inactiveUpSprite;
        }

        if (downRenderer)
        {
            bool downActive = currentLevel < maxLevel;
            downRenderer.sprite = downActive ? activeDownSprite : inactiveDownSprite;
        }
    }

    private void AddEventTriggers(GameObject target, UnityEngine.Events.UnityAction onClick)
    {
        var trigger = target.GetComponent<EventTrigger>();
        if (!trigger) trigger = target.AddComponent<EventTrigger>();

        Vector3 originalScale = target.transform.localScale;

        // PointerDown
        var downEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        downEntry.callback.AddListener((_) => { target.transform.localScale = originalScale * 0.9f; });
        trigger.triggers.Add(downEntry);

        // PointerUp
        var upEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        upEntry.callback.AddListener((_) => { target.transform.localScale = originalScale; });
        trigger.triggers.Add(upEntry);

        // PointerClick
        var clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        clickEntry.callback.AddListener((_) =>
        {
            if (target == scrollUpObject && currentLevel > minLevel)
                onClick.Invoke();
            else if (target == scrollDownObject && currentLevel < maxLevel)
                onClick.Invoke();
        });
        trigger.triggers.Add(clickEntry);
    }

    public void ScrollUp()
    {
        if (currentLevel > minLevel)
        {
            currentLevel--;
            scrollingController.ScrollToLevel(currentLevel);
            UpdateVisualState();
        }
    }

    public void ScrollDown()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            scrollingController.ScrollToLevel(currentLevel);
            UpdateVisualState();
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 90, 200, 30), $"Current Level: {currentLevel}");
    }
}

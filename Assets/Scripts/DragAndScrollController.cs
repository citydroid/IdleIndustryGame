using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndScrollController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("��������� ����������")]
    public float levelOffset = 50f;
    public float smoothTime = 0.3f;
    public float maxScrollSpeed = 1000f;
    public float snapThreshold = 20f; // ����� ��� ������������ � ������

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private int currentLevel = 0;
    private int maxReachedLevel = 0;
    private float targetOffset = 0f;
    private float currentOffset = 0f;
    private float currentVelocity = 0f;
    private bool isDragging = false;
    private Vector2 dragStartPosition;
    private float dragStartOffset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (!isDragging)
        {
            // �������������� ������� �����������
            currentOffset = Mathf.SmoothDamp(
                currentOffset,
                targetOffset,
                ref currentVelocity,
                smoothTime,
                maxScrollSpeed
            );

            UpdatePosition();
        }
    }

    // === ��������� �������������� ����� ===
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        isDragging = true;
        dragStartPosition = eventData.position;
        dragStartOffset = currentOffset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || eventData.button != PointerEventData.InputButton.Left) return;

        // ��������� �������� �� ������ ��������������
        float dragDelta = (eventData.position.y - dragStartPosition.y) / GetScaleFactor();
        float newOffset = dragStartOffset - dragDelta;

        // ������������ ����������� � ���������� ��������
        newOffset = Mathf.Clamp(newOffset, 0f, maxReachedLevel * levelOffset);

        currentOffset = newOffset;
        UpdatePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        isDragging = false;

        // ���������� ��������� ������� ��� ������������
        int closestLevel = Mathf.RoundToInt(currentOffset / levelOffset);
        closestLevel = Mathf.Clamp(closestLevel, 0, maxReachedLevel);

        // ����������� ������ ���� ������ � ������
        if (Mathf.Abs(currentOffset - closestLevel * levelOffset) <= snapThreshold)
        {
            currentLevel = closestLevel;
            targetOffset = currentLevel * levelOffset;
        }
        else
        {
            // ��������� �� ������� ������� (�� � �������� ������)
            currentLevel = Mathf.Clamp(Mathf.FloorToInt(currentOffset / levelOffset), 0, maxReachedLevel);
            targetOffset = Mathf.Clamp(currentOffset, 0f, maxReachedLevel * levelOffset);
        }
    }

    // === ���������� �������� ===
    public void GoToNextLevel()
    {
        currentLevel++;
        maxReachedLevel = Mathf.Max(maxReachedLevel, currentLevel);
        targetOffset = currentLevel * levelOffset;
    }

    public void ResetToTop()
    {
        currentLevel = 0;
        targetOffset = 0f;
    }

    public void ScrollToLevel(int level)
    {
        currentLevel = Mathf.Max(0, level);
        maxReachedLevel = Mathf.Max(maxReachedLevel, currentLevel);
        targetOffset = currentLevel * levelOffset;
    }

    // === ��������������� ������ ===
    private void UpdatePosition()
    {
        rectTransform.anchoredPosition = originalPosition - new Vector2(0, currentOffset);
    }

    private float GetScaleFactor()
    {
        // ��� ���������� ������ � ������ �����������
        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.scaleFactor : 1f;
    }

    // ��� ������� (����� �������)
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Next Level"))
        {
            GoToNextLevel();
        }

        if (GUI.Button(new Rect(10, 50, 150, 30), "Reset"))
        {
            ResetToTop();
        }

        GUI.Label(new Rect(10, 90, 300, 30), $"Current Level: {currentLevel}");
        GUI.Label(new Rect(10, 120, 300, 30), $"Max Level: {maxReachedLevel}");
    }
}
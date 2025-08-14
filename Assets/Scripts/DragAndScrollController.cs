using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndScrollController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки скроллинга")]
    [SerializeField] public List<float> levelOffsets = new List<float> { 0f, 50f, 80f, 120f };
    public float smoothTime = 0.3f;
    public float maxScrollSpeed = 1000f;
    public float snapThreshold = 20f;

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

    // ==== Публичные методы для управления уровнем ====
    public void GoToNextLevel()
    {
        if (currentLevel + 1 < levelOffsets.Count)
        {
            currentLevel++;
            maxReachedLevel = Mathf.Max(maxReachedLevel, currentLevel);
            targetOffset = GetTotalOffsetForLevel(currentLevel);
        }
    }

    public void ResetToTop()
    {
        currentLevel = 0;
        targetOffset = 0f;
    }

    public void ScrollToLevel(int level)
    {
        if (level >= 0 && level < levelOffsets.Count)
        {
            currentLevel = level;
            maxReachedLevel = Mathf.Max(maxReachedLevel, currentLevel);
            targetOffset = GetTotalOffsetForLevel(currentLevel);
        }
    }

    public int GetCurrentLevel() => currentLevel;
    public int GetMaxReachedLevel() => maxReachedLevel;

    // === Drag events ===
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

        float dragDelta = (eventData.position.y - dragStartPosition.y) / GetScaleFactor();
        float newOffset = dragStartOffset + dragDelta;

        float maxOffset = GetTotalOffsetForLevel(maxReachedLevel);
        newOffset = Mathf.Clamp(newOffset, 0f, maxOffset);

        currentOffset = newOffset;
        UpdatePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        isDragging = false;

        int closestLevel = FindClosestLevel(currentOffset);
        closestLevel = Mathf.Clamp(closestLevel, 0, maxReachedLevel);

        if (Mathf.Abs(currentOffset - GetTotalOffsetForLevel(closestLevel)) <= snapThreshold)
        {
            currentLevel = closestLevel;
            targetOffset = GetTotalOffsetForLevel(currentLevel);
        }
        else
        {
            currentLevel = FindCurrentLevel(currentOffset);
            targetOffset = Mathf.Clamp(currentOffset, 0f, GetTotalOffsetForLevel(maxReachedLevel));
        }
    }

    // === Helpers ===
    private void UpdatePosition()
    {
        rectTransform.anchoredPosition = originalPosition + new Vector2(0, currentOffset);
    }

    private float GetScaleFactor()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.scaleFactor : 1f;
    }

    private float GetTotalOffsetForLevel(int level)
    {
        if (level <= 0) return 0f;
        if (level >= levelOffsets.Count) return GetTotalOffsetForLevel(levelOffsets.Count - 1);

        float total = 0f;
        for (int i = 1; i <= level; i++)
        {
            if (i < levelOffsets.Count)
                total += levelOffsets[i];
        }
        return total;
    }

    private int FindClosestLevel(float offset)
    {
        float minDistance = float.MaxValue;
        int closestLevel = 0;

        for (int i = 0; i <= maxReachedLevel; i++)
        {
            float levelOffset = GetTotalOffsetForLevel(i);
            float distance = Mathf.Abs(offset - levelOffset);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestLevel = i;
            }
        }
        return closestLevel;
    }

    private int FindCurrentLevel(float offset)
    {
        for (int i = maxReachedLevel; i >= 0; i--)
        {
            if (offset >= GetTotalOffsetForLevel(i))
                return i;
        }
        return 0;
    }
}

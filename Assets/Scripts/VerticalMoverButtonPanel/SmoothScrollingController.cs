using UnityEngine;

public class SmoothScrollingController : MonoBehaviour
{
    [Header("��������� ����������")]
    [Tooltip("�������� ���� ��� ������� ������ ������ (� ��������)")]
    public float levelOffset = 50f;

    [Tooltip("����� �������� ���������� (� ��������)")]
    public float smoothTime = 0.3f;

    [Tooltip("������������ �������� ���������� (�������� � �������)")]
    public float maxScrollSpeed = 1000f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private int currentLevel = 0;
    private float targetOffset = 0f;
    private float currentOffset = 0f;
    private float currentVelocity = 0f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
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

    public void ScrollToLevel(int level)
    {
        currentLevel = Mathf.Max(0, level);
        targetOffset = currentLevel * levelOffset;
    }

    private void UpdatePosition()
    {
        rectTransform.anchoredPosition = originalPosition - new Vector2(0, currentOffset);
    }

}

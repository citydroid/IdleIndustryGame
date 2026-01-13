using UnityEngine;

public class BackgroundVerticalDrag : MonoBehaviour
{
    private Vector2 lastPosition;
    private bool isDragging;

    [Header("Настройки чувствительности")]
    public float dragSpeed = 0.01f;

    [Header("Ограничения движения по оси Y")]
    public float minY = -3f;
    public float maxY = 3f;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseDrag();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchDrag();
#endif
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastPosition = (Vector2)Input.mousePosition; // приведение к Vector2
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 currentPos = (Vector2)Input.mousePosition;
            Vector2 delta = currentPos - lastPosition;
            float moveY = -delta.y * dragSpeed; // Только вертикальное движение
            transform.position += new Vector3(0f, moveY, 0f);

            // Ограничиваем по Y
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y, minY, maxY),
                transform.position.z
            );

            lastPosition = currentPos;
        }
    }

    void HandleTouchDrag()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 currentPos = touch.position;
                Vector2 delta = currentPos - lastPosition;
                float moveY = -delta.y * dragSpeed; // Только вертикальное движение
                transform.position += new Vector3(0f, moveY, 0f);

                // Ограничиваем по Y
                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Clamp(transform.position.y, minY, maxY),
                    transform.position.z
                );

                lastPosition = currentPos;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
}

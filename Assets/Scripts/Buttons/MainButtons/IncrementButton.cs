using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class IncrementButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;
    public Color pressedColor = Color.white;

    private SpriteRenderer spriteRenderer;
    private bool isPointerOver = false;
    private IncrementChanger incrementChanger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
        incrementChanger = GetComponent<IncrementChanger>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;

        // Вызываем метод для обработки нажатия на неактивную кнопку
        if (incrementChanger != null)
        {
            incrementChanger.OnPointerDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPointerOver)
        {
            onClick?.Invoke();
        }

        spriteRenderer.color = isPointerOver ? hoverColor : normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        spriteRenderer.color = hoverColor;

        // Уведомляем IncrementChanger о наведении
        if (incrementChanger != null)
        {
            incrementChanger.OnPointerEnter();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        spriteRenderer.color = normalColor;

        // Уведомляем IncrementChanger о выходе курсора
        if (incrementChanger != null)
        {
            incrementChanger.OnPointerExit();
        }
    }
}
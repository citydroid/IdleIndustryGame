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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        spriteRenderer.color = normalColor;
    }
}
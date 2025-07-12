using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

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

    public Sprite activeSprite;
    public Sprite inactiveSprite;

    public float pressScale = 1f;
    public float pressDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private bool isPointerOver = false;
    private IncrementChanger incrementChanger;

    private Vector3 originalScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
        incrementChanger = GetComponent<IncrementChanger>();

        originalScale = Vector3.one;
        transform.localScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;

        // Вызов действия нажатия (например, показа инфо)
        incrementChanger?.OnPointerDown();

        // Только если кнопка активна (можно позволить покупку)
        if (incrementChanger != null && incrementChanger.CanAffordPublic())
        {
            StartCoroutine(PressEffect());
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
        incrementChanger?.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        spriteRenderer.color = normalColor;
        incrementChanger?.OnPointerExit();
    }

    private IEnumerator PressEffect()
    {
        transform.localScale = originalScale * pressScale;
        yield return new WaitForSeconds(pressDuration);
        transform.localScale = originalScale;
    }
    public void ResetScale()
    {
        transform.localScale = originalScale;
    }
}

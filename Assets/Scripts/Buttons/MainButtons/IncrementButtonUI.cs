using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class IncrementButtonUI : MonoBehaviour,
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

    [SerializeField] private AudioClip clickSound; 
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.5f;
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private bool isPointerOver = false;
    private IncrementChanger incrementChanger;

    private Vector3 originalScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
        audioSource = GetComponent<AudioSource>();
        incrementChanger = GetComponent<IncrementChanger>();

        originalScale = Vector3.one;
        transform.localScale = originalScale;

        audioSource.playOnAwake = false;
        audioSource.volume = soundVolume; 
        if (clickSound != null)
        {
            audioSource.clip = clickSound; 
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;

        incrementChanger?.OnPointerDown();

        // Только если кнопка активна (можно позволить покупку)
        if (incrementChanger != null && incrementChanger.CanAffordPublic())
        {
            StartCoroutine(PressEffect());
        }

        if (incrementChanger != null && incrementChanger.CanAffordPublic() && clickSound != null)
        {
            audioSource.Play();
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
    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = soundVolume;
        }
    }
}



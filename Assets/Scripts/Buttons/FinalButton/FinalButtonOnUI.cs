using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class FinalButtonOnUI : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;
    public Color pressedColor = Color.white;

    [Header("Sprites")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    [Header("Press Animation")]
    public float pressScale = 1f;
    public float pressDuration = 0.1f;

    [Header("Sound")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.5f;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private FinalButtonOn finalButtonOn;

    private bool isPointerOver = false;
    private Vector3 originalScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        finalButtonOn = GetComponent<FinalButtonOn>();

        originalScale = Vector3.one;
        transform.localScale = originalScale;

        spriteRenderer.color = normalColor;

        audioSource.playOnAwake = false;
        audioSource.volume = soundVolume;

        if (clickSound != null)
            audioSource.clip = clickSound;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        spriteRenderer.color = hoverColor;
        finalButtonOn?.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        spriteRenderer.color = normalColor;
        finalButtonOn?.OnPointerExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;
        finalButtonOn?.OnPointerDown();

        if (finalButtonOn != null && finalButtonOn.IsActivated())
        {
            StartCoroutine(PressEffect());
            if (clickSound != null)
                audioSource.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.color = isPointerOver ? hoverColor : normalColor;

        if (finalButtonOn != null && finalButtonOn.IsActivated())
            finalButtonOn.OnButtonClick();
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
            audioSource.volume = soundVolume;
    }
}

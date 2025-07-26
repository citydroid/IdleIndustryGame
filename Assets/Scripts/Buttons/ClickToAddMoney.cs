using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // Для звука клика
public class ClickToAddMoney : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private MainScript mainScript;
    [SerializeField] private int clickReward = 1;
    [SerializeField] private float clickScaleFactor = 0.9f;
    [SerializeField] private float animationDuration = 0.1f;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject floatingTextPrefab; // Префаб всплывающего текста
    [SerializeField] private Vector2 textOffset = new Vector2(0, 1f);
    [SerializeField] private Color textColor = Color.yellow;
    [SerializeField] private float textLifetime = 1f;
    [SerializeField] private float textFloatSpeed = 1f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float soundVolume = 0.5f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem clickParticles;
    [SerializeField] private int particlesCount = 10;

    private Vector3 _originalScale;
    private bool _isAnimating = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _audioSource = GetComponent<AudioSource>();

        if (mainScript == null)
            mainScript = FindObjectOfType<MainScript>();

        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (_isAnimating) return;

        if (mainScript != null && mainScript.result != null)
        {
            // Добавляем деньги
            mainScript.result.TotalValue += clickReward;

            // Визуальные эффекты
            ShowFloatingText();
            PlayClickParticles();

            // Звуковой эффект
            PlayClickSound();

            // Анимация
            StartCoroutine(ClickAnimation());
        }
    }

    private void ShowFloatingText()
    {
        if (floatingTextPrefab != null)
        {
            Vector3 spawnPosition = transform.position + (Vector3)textOffset;
            GameObject textObj = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);
            TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();

            if (textMesh != null)
            {
                textMesh.text = $"+{clickReward}";
                textMesh.color = textColor;
            }

            StartCoroutine(FloatAndDestroy(textObj));
        }
    }

    private IEnumerator FloatAndDestroy(GameObject textObj)
    {
        float timer = 0f;
        Vector3 startPosition = textObj.transform.position;

        while (timer < textLifetime)
        {
            textObj.transform.position = startPosition + new Vector3(0, textFloatSpeed * timer, 0);
            timer += Time.deltaTime;

            // Плавное исчезновение
            if (textObj.TryGetComponent<TextMeshPro>(out var text))
            {
                Color color = text.color;
                color.a = 1f - (timer / textLifetime);
                text.color = color;
            }

            yield return null;
        }

        Destroy(textObj);
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
        {
            _audioSource.PlayOneShot(clickSound, soundVolume);
        }
    }

    private void PlayClickParticles()
    {
        if (clickParticles != null)
        {
            clickParticles.Emit(particlesCount);
        }
    }

    private IEnumerator ClickAnimation()
    {
        _isAnimating = true;

        // Уменьшаем объект
        transform.localScale = _originalScale * clickScaleFactor;

        // Ждем указанное время
        yield return new WaitForSeconds(animationDuration);

        // Возвращаем исходный размер
        transform.localScale = _originalScale;

        _isAnimating = false;
    }
}
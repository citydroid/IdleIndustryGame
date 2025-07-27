using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // Для звука клика
public class ClickToAddMoneyAvto : MonoBehaviour
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

            // Звук
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
            GameObject instance = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

            var floatingText = instance.GetComponent<FloatingText>();
            if (floatingText != null)
            {
                floatingText.Initialize(clickReward, textColor, textLifetime, textFloatSpeed);
            }
        }
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

        // Уменьшение объекта
        transform.localScale = _originalScale * clickScaleFactor;

        yield return new WaitForSeconds(animationDuration);

        // Возврат к исходному масштабу
        transform.localScale = _originalScale;

        _isAnimating = false;
    }
}

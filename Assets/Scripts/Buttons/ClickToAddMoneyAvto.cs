using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // Для звука клика
public class ClickToAddMoneyAvto : MonoBehaviour
{
    [Header("Main Settings")]
    private MainScript mainScript;
    private int clickReward = 15;
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

        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        // Автоматическое присвоение mainScript
        if (mainScript == null)
        {
            GameObject mainSystem = GameObject.Find("MainSystem");
            if (mainSystem != null)
            {
                Transform mainScriptObj = mainSystem.transform.Find("MainScriptObj");
                if (mainScriptObj != null)
                {
                    mainScript = mainScriptObj.GetComponent<MainScript>();
                }
                else
                {
                    Debug.LogWarning("MainScriptObj не найден в MainSystem!");
                }
            }
            else
            {
                Debug.LogWarning("MainSystem не найден на сцене!");
            }
        }
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
    public void SetClickReward(int value)
    {
        clickReward = value;
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

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ClicUFO : MonoBehaviour
{
    private MainScript mainScript;
    private UFOMovement manager;

    [Header("Префаб всплывающего текста 'x2'")]
    [SerializeField] private GameObject scalingFadeTextPrefab;
    [Header("Задержка перед появлением текста (секунды)")]
    [SerializeField] private float textSpawnDelay = 0.5f;

    [Header("Позиция для появления текста")]
    [SerializeField] private Transform textSpawnPoint;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float soundVolume = 2f;
    private AudioSource _audioSource;

    [Header("Анимация при уничтожении UFO")]
    [SerializeField] private GameObject animationPrefab;

    public void Init(UFOMovement manager, MainScript mainScript)
    {
        this.manager = manager;
        this.mainScript = mainScript;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.volume = soundVolume; 
        if (clickSound != null)
        {
            _audioSource.clip = clickSound;
        }
    }

    public void Execute()
    {
        if (mainScript != null && mainScript.result != null)
        {
            mainScript.result.TotalValue *= 2;
            mainScript.ForceUpdateValues();

            if (scalingFadeTextPrefab != null)
            {
                Vector3 spawnPos = textSpawnPoint != null ? textSpawnPoint.position : transform.position;
                GameObject obj = Instantiate(scalingFadeTextPrefab, spawnPos, Quaternion.identity);

                var scalingText = obj.GetComponent<ScalingFadeText>();
                if (scalingText != null)
                {
                    scalingText.Initialize("x2", Color.yellow);
                }
                else
                {
                    Debug.LogWarning("ClicUFOActivator: В префабе нет ScalingFadeText компонента");
                }
            }
        }

        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position, soundVolume);
        }

        //StartCoroutine(SpawnTextWithDelay());

        // Создание и управление анимацией
        if (animationPrefab != null)
        {
            GameObject animObj = Instantiate(animationPrefab, transform.position, Quaternion.identity);
            Animator animator = animObj.GetComponent<Animator>();

            if (animator != null)
            {
                animator.Play("YourAnimationName");
                float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
                Destroy(animObj, animLength);
            }
            else
            {
                Debug.LogError("Animator не найден на анимационном объекте!");
            }
        }

        if (manager != null)
        {
            manager.DestroyUFO(gameObject);
        }
    }
    private IEnumerator SpawnTextWithDelay()
    {
        yield return new WaitForSeconds(textSpawnDelay);

        if (scalingFadeTextPrefab != null)
        {
            Vector3 spawnPos = textSpawnPoint != null ? textSpawnPoint.position : transform.position;
            GameObject obj = Instantiate(scalingFadeTextPrefab, spawnPos, Quaternion.identity);

            var scalingText = obj.GetComponent<ScalingFadeText>();
            if (scalingText != null)
            {
                scalingText.Initialize("x2", Color.yellow);
            }
            else
            {
                Debug.LogWarning("ClicUFOActivator: В префабе нет ScalingFadeText компонента");
            }
        }
    }
    private void OnValidate()
    {
        if (_audioSource != null)
        {
            _audioSource.volume = soundVolume; 
        }
    }

    private void OnMouseDown()
    {
        Execute();
    }
}
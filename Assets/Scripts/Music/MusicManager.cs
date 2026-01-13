using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip introClip;
    public AudioClip mainThemeClip;
    public AudioClip melodyClip;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    [Tooltip("На сколько секунд раньше запускается следующий трек.")]
    public float overlapTime = 0.5f;
    [Tooltip("Длительность кроссфейда между треками.")]
    public float crossfadeDuration = 0.5f;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private bool useSourceA = true;
    private bool isPaused = false;
    private bool isStopped = false;

    private void Awake()
    {
        // создаем два аудиоисточника для перекрёстного затухания
        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();

        foreach (var src in new[] { sourceA, sourceB })
        {
            src.playOnAwake = false;
            src.loop = false;
            src.volume = 0f; // начнем с нуля
        }
    }

    private void Start()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // 1️⃣ Intro
        if (introClip != null)
        {
            yield return StartCoroutine(PlayClip(introClip));
        }

        // 2️⃣ Цикл: 2×Main + Melody
        while (!isStopped)
        {
            for (int i = 0; i < 2; i++)
            {
                if (isStopped) yield break;
                yield return StartCoroutine(PlayClip(mainThemeClip));
            }

            if (isStopped) yield break;
            yield return StartCoroutine(PlayClip(melodyClip));
        }
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        AudioSource current = useSourceA ? sourceA : sourceB;
        AudioSource next = useSourceA ? sourceB : sourceA;

        current.clip = clip;
        current.Play();
        StartCoroutine(FadeIn(current, crossfadeDuration));

        float timeToWait = clip.length - overlapTime;
        if (timeToWait < 0) timeToWait = clip.length;

        float timer = 0f;
        while (timer < timeToWait && !isStopped)
        {
            if (!isPaused)
                timer += Time.deltaTime;
            yield return null;
        }

        // Переключаем на другой источник для следующего трека
        useSourceA = !useSourceA;

        // Когда запустится следующий трек — плавно уменьшим громкость старого
        if (!isStopped)
            StartCoroutine(FadeOut(current, crossfadeDuration));
    }

    // Плавное нарастание громкости
    private IEnumerator FadeIn(AudioSource src, float duration)
    {
        float start = src.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            src.volume = Mathf.Lerp(start, volume, time / duration);
            yield return null;
        }
        src.volume = volume;
    }

    // Плавное затухание громкости
    private IEnumerator FadeOut(AudioSource src, float duration)
    {
        float start = src.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            src.volume = Mathf.Lerp(start, 0f, time / duration);
            yield return null;
        }
        src.volume = 0f;
    }

    // Управление
    public void PauseMusic()
    {
        if (!isPaused)
        {
            sourceA.Pause();
            sourceB.Pause();
            isPaused = true;
        }
    }

    public void ResumeMusic()
    {
        if (isPaused)
        {
            sourceA.UnPause();
            sourceB.UnPause();
            isPaused = false;
        }
    }

    public void StopMusic()
    {
        isStopped = true;
        sourceA.Stop();
        sourceB.Stop();
        StopAllCoroutines();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        sourceA.volume = volume;
        sourceB.volume = volume;
    }

    public AudioSource GetActiveSource() => useSourceA ? sourceA : sourceB;
}

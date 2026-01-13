using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MusicManager))]
public class MusicFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 2f; // время затухания в секундах

    private MusicManager musicManager;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        musicManager = GetComponent<MusicManager>();
    }

    public void FadeOutAndStop()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        AudioSource active = musicManager.GetActiveSource();
        AudioSource other = active == null ? null : (active == musicManager.GetActiveSource() ? null : active);

        float startVolA = active ? active.volume : 0f;
        float startVolB = other ? other.volume : 0f;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float newVol = Mathf.Lerp(1f, 0f, t / fadeDuration);
            if (active) active.volume = startVolA * newVol;
            if (other) other.volume = startVolB * newVol;
            yield return null;
        }

        if (active) active.volume = 0f;
        if (other) other.volume = 0f;
        musicManager.StopMusic();
    }
}

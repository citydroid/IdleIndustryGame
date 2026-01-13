using UnityEngine;
using System.Collections;

public class SpriteWipeController : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;

    [Header("Wipe Settings")]
    public float duration = 1.5f;
    public float borderWidth = 0.02f;

    [Range(0f, 1f)] public float startProgress = 0f;
    [Range(0f, 1f)] public float endProgress = 1f;
    public bool playOnStart = true;

    private Material matInstance;

    void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteWipeController: SpriteRenderer не назначен.");
            enabled = false;
            return;
        }

        matInstance = spriteRenderer.material;
        matInstance.SetFloat("_WipeProgress", startProgress);
        matInstance.SetFloat("_BorderWidth", borderWidth);

        if (playOnStart)
            StartWipe(startProgress, endProgress, duration);
    }

    public void StartWipe(float from, float to, float time)
    {
        StopAllCoroutines();
        StartCoroutine(WipeCoroutine(from, to, time));
    }

    IEnumerator WipeCoroutine(float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float prog = Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t / time));

            float finalProg = prog;

            matInstance.SetFloat("_WipeProgress", finalProg);
            yield return null;
        }

        float finalEnd = to;
        matInstance.SetFloat("_WipeProgress", finalEnd);
    }
}
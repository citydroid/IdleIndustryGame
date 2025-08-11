using UnityEngine;
using TMPro;
using System.Collections;

public class ScalingFadeText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; // Название объекта с текстом внутри префаба
    [SerializeField] private float startScale = 1f;             // Начальный размер
    [SerializeField] private float endScale = 2f;               // Конечный размер
    [SerializeField] private float lifetime = 1f;               // Время жизни

    private TextMeshPro textMesh;

    /// <summary>
    /// Инициализация эффекта текста.
    /// </summary>
    public void Initialize(string text, Color color)
    {
        // Находим объект с текстом
        Transform textTransform = transform.Find(textObjectName);
        if (textTransform != null)
        {
            textMesh = textTransform.GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = text;
            textMesh.color = color;
            textMesh.transform.localScale = Vector3.one * startScale;
        }

        StartCoroutine(ScaleAndFade());
    }

    /// <summary>
    /// Эффект увеличения и исчезновения.
    /// </summary>
    private IEnumerator ScaleAndFade()
    {
        float timer = 0f;

        while (timer < lifetime)
        {
            float t = timer / lifetime;

            // Увеличиваем размер
            if (textMesh != null)
            {
                float scale = Mathf.Lerp(startScale, endScale, t);
                textMesh.transform.localScale = Vector3.one * scale;

                // Уменьшаем прозрачность
                Color c = textMesh.color;
                c.a = 1f - t;
                textMesh.color = c;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

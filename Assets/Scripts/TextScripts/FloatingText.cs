using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; // Название объекта с текстом внутри префаба

    private TextMeshPro textMesh;

    /// <summary>
    /// Инициализация всплывающего текста.
    /// </summary>
    public void Initialize(int value, Color color, float lifetime, float floatSpeed)
    {
        Transform textTransform = transform.Find(textObjectName);
        if (textTransform != null)
        {
            textMesh = textTransform.GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = $"+{value}";
            textMesh.color = color;
        }

        StartCoroutine(FloatAndFade(lifetime, floatSpeed));
    }

    /// <summary>
    /// Анимация плавного подъема и исчезновения текста.
    /// </summary>
    private IEnumerator FloatAndFade(float lifetime, float floatSpeed)
    {
        float timer = 0f;
        Vector3 startPosition = transform.position;

        while (timer < lifetime)
        {
            transform.position = startPosition + new Vector3(0, floatSpeed * timer, 0);
            timer += Time.deltaTime;

            if (textMesh != null)
            {
                Color c = textMesh.color;
                c.a = 1f - (timer / lifetime);
                textMesh.color = c;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}

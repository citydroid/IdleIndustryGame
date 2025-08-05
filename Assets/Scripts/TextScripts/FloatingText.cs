using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; // Название объекта с текстом внутри префаба
    [SerializeField] private string coinObjectName = "Coin";    // Название объекта монетки (дочернего)

    private TextMeshPro textMesh;
    private GameObject coinObject; // Ссылка на объект монетки

    /// <summary>
    /// Инициализация всплывающего текста.
    /// </summary>
    public void Initialize(int value, Color color, float lifetime, float floatSpeed)
    {
        // Находим текстовый объект
        Transform textTransform = transform.Find(textObjectName);
        if (textTransform != null)
        {
            textMesh = textTransform.GetComponent<TextMeshPro>();
        }

        // Находим объект монетки (если есть)
        Transform coinTransform = transform.Find(coinObjectName);
        if (coinTransform != null)
        {
            coinObject = coinTransform.gameObject;
        }

        if (textMesh != null)
        {
            textMesh.text = $"{value}"; // Добавляем "+" перед числом
            textMesh.color = color;
        }

        StartCoroutine(FloatAndFade(lifetime, floatSpeed));
    }

    /// <summary>
    /// Анимация плавного подъема и исчезновения текста и монетки.
    /// </summary>
    private IEnumerator FloatAndFade(float lifetime, float floatSpeed)
    {
        float timer = 0f;
        Vector3 startPosition = transform.position;

        while (timer < lifetime)
        {
            // Плавное движение вверх
            transform.position = startPosition + new Vector3(0, floatSpeed * timer, 0);
            timer += Time.deltaTime;

            // Плавное исчезновение текста
            if (textMesh != null)
            {
                Color textColor = textMesh.color;
                textColor.a = 1f - (timer / lifetime);
                textMesh.color = textColor;
            }

            // Плавное исчезновение монетки (если она есть)
            if (coinObject != null)
            {
                SpriteRenderer coinRenderer = coinObject.GetComponent<SpriteRenderer>();
                if (coinRenderer != null)
                {
                    Color coinColor = coinRenderer.color;
                    coinColor.a = 1f - (timer / lifetime);
                    coinRenderer.color = coinColor;
                }
            }

            yield return null;
        }

        Destroy(gameObject); // Уничтожаем весь префаб
    }
}
using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; 
    [SerializeField] private string coinObjectName = "Coin1";    
    [SerializeField] private string coinObjectName2 = "Coin2";
    [SerializeField] private bool useSecondCoin = false;  

    private TextMeshPro textMesh;
    private GameObject coinObject; 
    private GameObject coinObject2;

    public void Initialize(int value, Color color, float lifetime, float floatSpeed)
    {

        Transform textTransform = transform.Find(textObjectName);
        if (textTransform != null)
        {
            textMesh = textTransform.GetComponent<TextMeshPro>();
        }

        Transform coinTransform = transform.Find(coinObjectName);
        if (coinTransform != null)
        {
            coinObject = coinTransform.gameObject;
        }

        if (useSecondCoin)
        {
            Transform coinTransform2 = transform.Find(coinObjectName2);
            if (coinTransform2 != null)
                coinObject2 = coinTransform2.gameObject;
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
            float alpha = 1f - (timer / lifetime);

            // Плавное исчезновение текста
            if (textMesh != null)
            {
                Color textColor = textMesh.color;
                textColor.a = alpha;
                textMesh.color = textColor;
            }

            // Плавное исчезновение монетки (если она есть)
            FadeCoin(coinObject, alpha);

            if (useSecondCoin)
                FadeCoin(coinObject2, alpha);

            yield return null;
        }

        Destroy(gameObject); // Уничтожаем весь префаб
    }
    private void FadeCoin(GameObject coin, float alpha)
    {
        if (coin == null) return;

        SpriteRenderer coinRenderer = coin.GetComponent<SpriteRenderer>();
        if (coinRenderer != null)
        {
            Color coinColor = coinRenderer.color;
            coinColor.a = alpha;
            coinRenderer.color = coinColor;
        }
    }
}
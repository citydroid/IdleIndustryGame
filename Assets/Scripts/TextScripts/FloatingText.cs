using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; // �������� ������� � ������� ������ �������

    private TextMeshPro textMesh;

    /// <summary>
    /// ������������� ������������ ������.
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
    /// �������� �������� ������� � ������������ ������.
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

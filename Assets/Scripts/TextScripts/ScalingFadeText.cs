using UnityEngine;
using TMPro;
using System.Collections;

public class ScalingFadeText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; // �������� ������� � ������� ������ �������
    [SerializeField] private float startScale = 1f;             // ��������� ������
    [SerializeField] private float endScale = 2f;               // �������� ������
    [SerializeField] private float lifetime = 1f;               // ����� �����

    private TextMeshPro textMesh;

    /// <summary>
    /// ������������� ������� ������.
    /// </summary>
    public void Initialize(string text, Color color)
    {
        // ������� ������ � �������
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
    /// ������ ���������� � ������������.
    /// </summary>
    private IEnumerator ScaleAndFade()
    {
        float timer = 0f;

        while (timer < lifetime)
        {
            float t = timer / lifetime;

            // ����������� ������
            if (textMesh != null)
            {
                float scale = Mathf.Lerp(startScale, endScale, t);
                textMesh.transform.localScale = Vector3.one * scale;

                // ��������� ������������
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

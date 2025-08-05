using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private string textObjectName = "TextObj"; // �������� ������� � ������� ������ �������
    [SerializeField] private string coinObjectName = "Coin";    // �������� ������� ������� (���������)

    private TextMeshPro textMesh;
    private GameObject coinObject; // ������ �� ������ �������

    /// <summary>
    /// ������������� ������������ ������.
    /// </summary>
    public void Initialize(int value, Color color, float lifetime, float floatSpeed)
    {
        // ������� ��������� ������
        Transform textTransform = transform.Find(textObjectName);
        if (textTransform != null)
        {
            textMesh = textTransform.GetComponent<TextMeshPro>();
        }

        // ������� ������ ������� (���� ����)
        Transform coinTransform = transform.Find(coinObjectName);
        if (coinTransform != null)
        {
            coinObject = coinTransform.gameObject;
        }

        if (textMesh != null)
        {
            textMesh.text = $"{value}"; // ��������� "+" ����� ������
            textMesh.color = color;
        }

        StartCoroutine(FloatAndFade(lifetime, floatSpeed));
    }

    /// <summary>
    /// �������� �������� ������� � ������������ ������ � �������.
    /// </summary>
    private IEnumerator FloatAndFade(float lifetime, float floatSpeed)
    {
        float timer = 0f;
        Vector3 startPosition = transform.position;

        while (timer < lifetime)
        {
            // ������� �������� �����
            transform.position = startPosition + new Vector3(0, floatSpeed * timer, 0);
            timer += Time.deltaTime;

            // ������� ������������ ������
            if (textMesh != null)
            {
                Color textColor = textMesh.color;
                textColor.a = 1f - (timer / lifetime);
                textMesh.color = textColor;
            }

            // ������� ������������ ������� (���� ��� ����)
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

        Destroy(gameObject); // ���������� ���� ������
    }
}
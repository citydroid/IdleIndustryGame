using UnityEngine;

public class CarBodyBob : MonoBehaviour
{
    [SerializeField] private float bobAmplitude = 0.05f; // ��������� �����������
    [SerializeField] private float bobFrequency = 2f;    // ������� (���-�� ��������� � �������)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * bobFrequency * Mathf.PI * 2) * bobAmplitude;
        transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }
}

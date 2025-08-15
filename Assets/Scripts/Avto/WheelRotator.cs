using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f; // �������� � �������

    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}

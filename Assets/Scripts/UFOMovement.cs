using UnityEngine;

public class UFOMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float wobbleHeight = 0.2f;
    public float wobbleSpeed = 5f;

    [Header("Tilt Settings")]
    public float maxTiltAngle = 15f;
    public float tiltChangeInterval = 3f;
    public float tiltSpeed = 2f;

    private Vector3 currentTarget;
    private float tiltTimer;
    private float targetTilt;
    private float currentTilt;
    private float startY;
    private bool isMovingToB = true;

    void Start()
    {
        // ������������� ��������� ����
        currentTarget = pointB.position;
        startY = transform.position.y;

        // �������������� ������ �������
        tiltTimer = tiltChangeInterval;
        targetTilt = 0f;
        currentTilt = 0f;
    }

    void Update()
    {
        // ����������� ����� �������
        MoveBetweenPoints();

        // ����������� �����-����
        ApplyWobble();

        // ��������� �������
        HandleTilting();
    }

    void MoveBetweenPoints()
    {
        // ��������� � ������� ����
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // ���� �������� ����, ������ ����
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (isMovingToB)
            {
                currentTarget = pointA.position;
            }
            else
            {
                currentTarget = pointB.position;
            }
            isMovingToB = !isMovingToB;
        }
    }

    void ApplyWobble()
    {
        // �������������� ����������� �����-����
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight;
        Vector3 pos = transform.position;
        pos.y = startY + wobble;
        transform.position = pos;
    }

    void HandleTilting()
    {
        // ��������� ������
        tiltTimer -= Time.deltaTime;

        // ���� ������ �����, �������� ����� ���� �������
        if (tiltTimer <= 0f)
        {
            tiltTimer = tiltChangeInterval;
            targetTilt = Random.Range(-maxTiltAngle, maxTiltAngle);
        }

        // ������ �������� ������� ���� � ��������
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);

        // ��������� ������� ������ ��� Z (� 2D ��� ����� ��������� ��� ������)
        transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
}
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

    private MainScript mainScript;

    void Start()
    {
        currentTarget = pointB.position;
        startY = transform.position.y;

        tiltTimer = tiltChangeInterval;
        targetTilt = 0f;
        currentTilt = 0f;

        // Поиск MainScript на сцене
        mainScript = FindObjectOfType<MainScript>();
        if (mainScript == null)
        {
            Debug.LogWarning("MainScript not found in scene.");
        }
    }

    void Update()
    {
        MoveBetweenPoints();
        ApplyWobble();
        HandleTilting();
    }

    void MoveBetweenPoints()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            currentTarget = isMovingToB ? pointA.position : pointB.position;
            isMovingToB = !isMovingToB;
        }
    }

    void ApplyWobble()
    {
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight;
        Vector3 pos = transform.position;
        pos.y = startY + wobble;
        transform.position = pos;
    }

    void HandleTilting()
    {
        tiltTimer -= Time.deltaTime;

        if (tiltTimer <= 0f)
        {
            tiltTimer = tiltChangeInterval;
            targetTilt = Random.Range(-maxTiltAngle, maxTiltAngle);
        }

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    void OnMouseDown()
    {
        if (mainScript != null && mainScript.result != null)
        {
            mainScript.result.TotalValue *= 2;
           // mainScript.saveSystem.SaveTotalValue(mainScript.result.TotalValue);
            mainScript.ForceUpdateValues();
        }
    }
}

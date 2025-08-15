using UnityEngine;
using System.Collections.Generic;

public class AvtoMovingController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private GameObject prefabToMove;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypointsA;
    [SerializeField] private List<Transform> waypointsB;

    [Header("Rotation Settings")]
    [SerializeField] private List<float> prefabRotations; // 🔹 Углы наклона для каждой пары точек

    [Header("Parent Reference")]
    [SerializeField] private Transform parentReference;

    private Vector3 localTargetPos;
    private int currentLevel = 0;
    private bool movingForward = true;
    private SpriteRenderer spriteRenderer;

    [Header("Level Control")]
    [SerializeField] private int numberLevel = 0; // 🔹 Максимальный разрешённый сегмент (0 = A0-B0)

    private void Start()
    {
        if (prefabToMove == null)
        {
            Debug.LogError("Prefab to move is not assigned!");
            return;
        }

        if (parentReference == null)
        {
            Debug.LogError("Parent Reference is not assigned!");
            return;
        }

        spriteRenderer = prefabToMove.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Prefab doesn't have SpriteRenderer component!");
        }

        if (prefabRotations.Count < waypointsA.Count)
        {
            Debug.LogWarning("Количество углов меньше количества уровней! Заполните список prefabRotations в инспекторе.");
        }

        // Начало движения
        SetTargetLocal(waypointsA[currentLevel].localPosition, waypointsB[currentLevel].localPosition, prefabRotations[currentLevel]);
    }

    private void Update()
    {
        if (prefabToMove == null) return;

        Vector3 worldTargetPos = parentReference.TransformPoint(localTargetPos);

        prefabToMove.transform.position = Vector3.MoveTowards(
            prefabToMove.transform.position,
            worldTargetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(prefabToMove.transform.position, worldTargetPos) < 0.01f)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            HandleWaypointReached();
        }
    }

    private void HandleWaypointReached()
    {
        if (movingForward)
        {
            // 🚩 Мы на точке B
            if (localTargetPos == waypointsB[currentLevel].localPosition)
            {
                if (currentLevel >= numberLevel) // Достигли максимального разрешённого сегмента
                {
                    movingForward = false;
                    SetTargetLocal(waypointsB[currentLevel].localPosition, waypointsA[currentLevel].localPosition, prefabRotations[currentLevel]);
                    return;
                }
                else
                {
                    // Переходим на следующий сегмент
                    currentLevel++;
                    SetTargetLocal(waypointsA[currentLevel].localPosition, waypointsB[currentLevel].localPosition, prefabRotations[currentLevel]);
                    return;
                }
            }
        }
        else
        {
            // 🚩 Мы на точке A
            if (localTargetPos == waypointsA[currentLevel].localPosition)
            {
                if (currentLevel > 0)
                {
                    currentLevel--;
                    SetTargetLocal(waypointsB[currentLevel].localPosition, waypointsA[currentLevel].localPosition, prefabRotations[currentLevel]);
                }
                else
                {
                    movingForward = true;
                    SetTargetLocal(waypointsA[currentLevel].localPosition, waypointsB[currentLevel].localPosition, prefabRotations[currentLevel]);
                }
            }
        }
    }

    private void SetTargetLocal(Vector3 startLocalPos, Vector3 endLocalPos, float rotationZ)
    {
        prefabToMove.transform.position = parentReference.TransformPoint(startLocalPos);
        prefabToMove.transform.rotation = Quaternion.Euler(0, 0, rotationZ); // 🔹 Применяем угол
        localTargetPos = endLocalPos;
    }

    public void SetLevel(int newLevel)
    {
        numberLevel = Mathf.Clamp(newLevel, 0, waypointsA.Count - 1);
    }
}

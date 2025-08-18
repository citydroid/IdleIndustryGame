using UnityEngine;
using System.Collections.Generic;

public class AvtoMovingController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 0.3f;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> prefabsToMove;
    [SerializeField] private int currentPrefabIndex = 0;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypointsA;
    [SerializeField] private List<Transform> waypointsB;

    [Header("Rotation Settings")]
    [SerializeField] private List<float> prefabRotations;

    [Header("Parent Reference")]
    [SerializeField] private Transform parentReference;

    private GameObject activePrefab;
    private Vector3 localTargetPos;
    private int currentLevel = 0;
    private bool movingForward = true;

    [Header("Level Control")]
    [SerializeField] private int numberLevel = 0;

    private void Start()
    {
        if (prefabsToMove == null || prefabsToMove.Count == 0)
        {
            Debug.LogError("Prefabs list is empty!");
            return;
        }

        currentPrefabIndex = Mathf.Clamp(currentPrefabIndex, 0, prefabsToMove.Count - 1);
        UpdateActivePrefab();

        if (prefabRotations.Count < waypointsA.Count)
            Debug.LogWarning("Количество углов меньше количества уровней!");

        SetTargetLocal(waypointsA[currentLevel].localPosition, waypointsB[currentLevel].localPosition, prefabRotations[currentLevel]);
    }

    private void Update()
    {
        if (activePrefab == null) return;

        Vector3 worldTargetPos = parentReference.TransformPoint(localTargetPos);
        activePrefab.transform.position = Vector3.MoveTowards(
            activePrefab.transform.position,
            worldTargetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(activePrefab.transform.position, worldTargetPos) < 0.01f)
        {
            FlipBranch();
            HandleWaypointReached();
        }
    }

    private void HandleWaypointReached()
    {
        if (movingForward)
        {
            if (localTargetPos == waypointsB[currentLevel].localPosition)
            {
                if (currentLevel >= numberLevel)
                {
                    movingForward = false;
                    SetTargetLocal(waypointsB[currentLevel].localPosition, waypointsA[currentLevel].localPosition, prefabRotations[currentLevel]);
                }
                else
                {
                    currentLevel++;
                    SetTargetLocal(waypointsA[currentLevel].localPosition, waypointsB[currentLevel].localPosition, prefabRotations[currentLevel]);
                }
            }
        }
        else
        {
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

    private void FlipBranch()
    {
        if (activePrefab == null) return;

        Transform toA = activePrefab.transform.Find("ToA");
        Transform toB = activePrefab.transform.Find("ToB");
        if (toA == null || toB == null)
        {
            Debug.LogError("Active prefab должен содержать дочерние объекты 'ToA' и 'ToB'!");
            return;
        }

        // Переключаем активность при достижении любой точки
        bool isToAActive = toA.gameObject.activeSelf;
        toA.gameObject.SetActive(!isToAActive);
        toB.gameObject.SetActive(isToAActive);
    }


    private void SetTargetLocal(Vector3 startLocalPos, Vector3 endLocalPos, float rotationZ)
    {
        if (activePrefab == null) return;
        activePrefab.transform.position = parentReference.TransformPoint(startLocalPos);
        activePrefab.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        localTargetPos = endLocalPos;
    }

    public void SetLevel(int newLevel)
    {
        numberLevel = Mathf.Clamp(newLevel, 0, waypointsA.Count - 1);
    }

    public void SetCurrentPrefab(int index)
    {
        if (prefabsToMove == null || prefabsToMove.Count == 0) return;

        index = Mathf.Clamp(index, 0, prefabsToMove.Count - 1);
        if (index == currentPrefabIndex) return;

        GameObject oldPrefab = activePrefab;
        bool isToAActive = false;
        float oldRotationZ = 0f;

        if (oldPrefab != null)
        {
            Transform oldToA = oldPrefab.transform.Find("ToA");
            Transform oldToB = oldPrefab.transform.Find("ToB");
            if (oldToA != null && oldToB != null)
                isToAActive = oldToA.gameObject.activeSelf;

            // Сохраняем угол старого префаба
            oldRotationZ = oldPrefab.transform.rotation.eulerAngles.z;

            oldPrefab.SetActive(false);
        }

        currentPrefabIndex = index;
        UpdateActivePrefab();

        if (activePrefab != null)
        {
            // Сохраняем позицию старого префаба
            Vector3 oldPos = oldPrefab != null ? oldPrefab.transform.position : parentReference.TransformPoint(waypointsA[currentLevel].localPosition);
            activePrefab.transform.position = oldPos;

            // Устанавливаем угол нового префаба с учётом старого
            activePrefab.transform.rotation = Quaternion.Euler(0, 0, oldRotationZ);

            Transform toA = activePrefab.transform.Find("ToA");
            Transform toB = activePrefab.transform.Find("ToB");
            if (toA != null && toB != null)
            {
                toA.gameObject.SetActive(isToAActive);
                toB.gameObject.SetActive(!isToAActive);
            }
        }
    }



    private void UpdateActivePrefab()
    {
        if (prefabsToMove == null || prefabsToMove.Count == 0) return;

        activePrefab = prefabsToMove[currentPrefabIndex];
        if (activePrefab != null) activePrefab.SetActive(true);
    }
}

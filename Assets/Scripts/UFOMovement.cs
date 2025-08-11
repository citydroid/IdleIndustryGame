using UnityEngine;
using System.Collections;

public class UFOMovement : MonoBehaviour
{
    [Header("Prefab & Points")]
    [SerializeField] private GameObject ufoPrefab;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform ufoParent;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float wobbleHeight = 0.2f;
    [SerializeField] private float wobbleSpeed = 5f;
    [SerializeField] private float respawnDelay = 20f;

    [Header("Tilt Settings")]
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float tiltChangeInterval = 3f;
    [SerializeField] private float tiltSpeed = 2f;

    private GameObject currentUFO;
    private Vector3 currentTarget;
    private float tiltTimer;
    private float targetTilt;
    private float currentTilt;
    private float startY;
    private MainScript mainScript;
    private Vector3 localTarget;

    private void Start()
    {
        mainScript = FindObjectOfType<MainScript>();
        if (mainScript == null)
            Debug.LogWarning("MainScript not found in scene.");

        StartCoroutine(UFOSpawnLoop());
    }

    private IEnumerator UFOSpawnLoop()
    {
        while (true)
        {
            SpawnUFO();
            yield return new WaitUntil(() => currentUFO == null); // Ждём, пока UFO удалят
            yield return new WaitForSeconds(respawnDelay);        // Ждём перед новым спавном
        }
    }

    private void SpawnUFO()
    {
        if (ufoParent == null)
            ufoParent = this.transform;

        Vector3 localSpawnPos = ufoParent.InverseTransformPoint(pointA.position);
        localTarget = ufoParent.InverseTransformPoint(pointB.position);

        currentUFO = Instantiate(ufoPrefab, ufoParent);
        currentUFO.transform.localPosition = localSpawnPos;
        currentUFO.transform.localRotation = Quaternion.identity;

        tiltTimer = tiltChangeInterval;
        targetTilt = 0f;
        currentTilt = 0f;

        ClicUFO clickHandler = currentUFO.GetComponent<ClicUFO>();
        if (clickHandler != null)
        {
            clickHandler.Init(this, mainScript);
        }
    }

    private void Update()
    {
        if (currentUFO == null) return;

        MoveToTarget();
        ApplyWobble();
        HandleTilting();
    }

    private void MoveToTarget()
    {
        if (currentUFO == null) return;

        currentUFO.transform.localPosition = Vector3.MoveTowards(currentUFO.transform.localPosition, localTarget, speed * Time.deltaTime);

        if (Vector3.Distance(currentUFO.transform.localPosition, localTarget) < 0.01f)
        {
            Destroy(currentUFO);
            currentUFO = null;
        }
    }

    private void ApplyWobble()
    {
        if (currentUFO == null) return;

        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight;
        Vector3 pos = currentUFO.transform.localPosition;
        pos.y = (ufoParent.InverseTransformPoint(pointA.position)).y + wobble;
        currentUFO.transform.localPosition = pos;
    }

    private void HandleTilting()
    {
        if (currentUFO == null) return;

        tiltTimer -= Time.deltaTime;

        if (tiltTimer <= 0f)
        {
            tiltTimer = tiltChangeInterval;
            targetTilt = Random.Range(-maxTiltAngle, maxTiltAngle);
        }

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);
        currentUFO.transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
    public void DestroyUFO(GameObject ufo)
    {
        if (ufo != null)
        {
            Destroy(ufo);
            if (ufo == currentUFO)
                currentUFO = null;
        }
    }
}


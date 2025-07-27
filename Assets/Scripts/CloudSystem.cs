using UnityEngine;
using System.Collections;

public class CloudSystem : MonoBehaviour
{
    [Header("Cloud Settings")]
    [SerializeField] private GameObject[] cloudPrefabs; // ������ �������� �������
    [SerializeField] private float minSpawnInterval = 2f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private float minYPosition = 2f;
    [SerializeField] private float maxYPosition = 5f;
    [SerializeField] private float spawnXPosition = 10f; // ����� �� ������ �������� ������
    [SerializeField] private float destroyXPosition = -15f; // ����������� �� ����� ��������
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    private void Start()
    {
        StartCoroutine(SpawnCloudsRoutine());
    }

    private IEnumerator SpawnCloudsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnCloud();
        }
    }

    private void SpawnCloud()
    {
        if (cloudPrefabs.Length == 0) return;

        // �������� ��������� ������ ������
        GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

        // ��������� ������� �� Y
        float spawnYPosition = Random.Range(minYPosition, maxYPosition);
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, 0);

        // ������� ������
        GameObject cloud = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity, transform);

        // ������������� ��������� ���������
        float speed = Random.Range(minSpeed, maxSpeed);
        float scale = Random.Range(minScale, maxScale);
        cloud.transform.localScale = new Vector3(scale, scale, 1);

        // ��������� �������� ������
        StartCoroutine(MoveCloud(cloud, speed));
    }

    private IEnumerator MoveCloud(GameObject cloud, float speed)
    {
        SpriteRenderer renderer = cloud.GetComponent<SpriteRenderer>();

        // ������� ���������
        yield return StartCoroutine(FadeCloud(renderer, 0f, 1f, 0.5f));

        // �������� ������
        while (cloud != null && cloud.transform.position.x > destroyXPosition)
        {
            cloud.transform.Translate(Vector3.left * speed * Time.deltaTime);
            yield return null;
        }

        // ������� ������������ ����� ������������
        if (cloud != null)
        {
            yield return StartCoroutine(FadeCloud(renderer, 1f, 0f, 0.5f));
            Destroy(cloud);
        }
    }

    private IEnumerator FadeCloud(SpriteRenderer renderer, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            renderer.color = new Color(1, 1, 1, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        renderer.color = new Color(1, 1, 1, endAlpha);
    }

    // ����� ��� ��������������� ����������� ������ (����� �������� �� ������ ��������)
    public void DestroyCloud(GameObject cloud)
    {
        if (cloud != null)
        {
            StartCoroutine(FadeAndDestroy(cloud));
        }
    }

    private IEnumerator FadeAndDestroy(GameObject cloud)
    {
        SpriteRenderer renderer = cloud.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            yield return StartCoroutine(FadeCloud(renderer, 1f, 0f, 0.3f));
        }
        Destroy(cloud);
    }
}
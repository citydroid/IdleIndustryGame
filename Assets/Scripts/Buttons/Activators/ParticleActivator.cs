using UnityEngine;
using System.Collections;

public class ParticleActivator : MonoBehaviour, IButtonAction
{
    [Header("Основные настройки")]
    [SerializeField] private ParticleSystem particleSystemRef; // Система частиц
    [SerializeField] private Transform targetObject;           // Объект, относительно которого брать позицию
    [SerializeField] private Vector3 offset;                   // Дополнительное смещение

    private ParticleSystem.ShapeModule shapeModule;
    private Transform _transform;
    private float delayBeforeMove = 1f;
    private void Awake()
    {
        if (particleSystemRef == null)
            particleSystemRef = GetComponent<ParticleSystem>();

        if (particleSystemRef != null)
            shapeModule = particleSystemRef.shape;

        _transform = transform;

        // 🔹 Если объект с партиклами активен — выключаем его до вызова Execute()
        if (particleSystemRef != null)
            particleSystemRef.gameObject.SetActive(false);
    }

    public void Execute()
    {
        if (particleSystemRef == null || targetObject == null)
            return;

        // 🔹 Включаем объект с партиклами
        if (!particleSystemRef.gameObject.activeSelf)
            particleSystemRef.gameObject.SetActive(true);

        // 🔹 Запускаем систему
        if (!particleSystemRef.isPlaying)
            particleSystemRef.Play();

        // 🔹 Запускаем задержку перед сменой координат
        StartCoroutine(SetShapePositionWithDelay());
    }

    private IEnumerator SetShapePositionWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        if (particleSystemRef == null || targetObject == null)
            yield break;

        // 🔹 Пересчитываем позицию Shape после задержки
        Vector3 localPos = _transform.InverseTransformPoint(targetObject.position);
        shapeModule.position = localPos + offset;
    }
}

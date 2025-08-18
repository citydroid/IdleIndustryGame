using UnityEngine;

public class AvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("Объект с AvtoMovingController")]
    [SerializeField] private AvtoMovingController targetController;

    [Header("Индекс префаба (начиная с 0)")]
    [SerializeField] private int prefabIndex = 0;

    public void Execute()
    {
        Debug.Log($"PrefabAvtoActivator: Устанавливаем префаб с индексом {prefabIndex}");

        if (targetController == null)
        {
            Debug.LogWarning("PrefabAvtoActivator: targetController не назначен.");
            return;
        }

        targetController.SetCurrentPrefab(prefabIndex);
    }
}

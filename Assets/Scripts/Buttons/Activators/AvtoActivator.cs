using UnityEngine;
using System.Collections.Generic;

public class AvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("Объекты с AvtoMovingController")]
    [SerializeField] private List<AvtoMovingController> targetControllers = new List<AvtoMovingController>();

    [Header("Индекс префаба (начиная с 0)")]
    [SerializeField] private int prefabIndex = 0;

    public void Execute()
    {
        Debug.Log($"PrefabAvtoActivator: Устанавливаем префаб с индексом {prefabIndex} для {targetControllers.Count} контроллеров");

        if (targetControllers == null || targetControllers.Count == 0)
        {
            Debug.LogWarning("PrefabAvtoActivator: список targetControllers пуст.");
            return;
        }

        foreach (var controller in targetControllers)
        {
            if (controller != null)
            {
                controller.SetCurrentPrefab(prefabIndex);
            }
            else
            {
                Debug.LogWarning("PrefabAvtoActivator: Один из targetControllers не назначен.");
            }
        }
    }
}

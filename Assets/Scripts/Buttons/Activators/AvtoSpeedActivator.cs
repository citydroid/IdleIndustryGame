using UnityEngine;
using System.Collections.Generic;

public class AvtoSpeedActivator : MonoBehaviour, IButtonAction
{
    [Header("Объекты с AvtoMovingController")]
    [SerializeField] private List<AvtoMovingController> targetControllers = new List<AvtoMovingController>();

    [Header("Новая скорость движения")]
    [SerializeField] private float newSpeed = 0.5f;

    public void Execute()
    {
        Debug.Log($"AvtoSpeedActivator: Устанавливаем скорость {newSpeed} для {targetControllers.Count} контроллеров");

        if (targetControllers == null || targetControllers.Count == 0)
        {
            Debug.LogWarning("AvtoSpeedActivator: список targetControllers пуст.");
            return;
        }

        foreach (var controller in targetControllers)
        {
            if (controller != null)
            {
                controller.SetSpeed(newSpeed);
            }
            else
            {
                Debug.LogWarning("AvtoSpeedActivator: Один из targetControllers не назначен.");
            }
        }
    }
}

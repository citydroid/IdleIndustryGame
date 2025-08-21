using UnityEngine;
using System.Collections.Generic;

public class LevelAvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("Объекты с AvtoMovingController")]
    [SerializeField] private List<AvtoMovingController> targetControllers = new List<AvtoMovingController>();

    [Header("Номер уровня (начиная с 1)")]
    [SerializeField] private int levelToSet = 1;

    public void Execute()
    {
        Debug.Log($"LevelAvtoActivator: Устанавливаем уровень {levelToSet} для {targetControllers.Count} объектов");

        if (targetControllers == null || targetControllers.Count == 0)
        {
            Debug.LogWarning("LevelAvtoActivator: список targetControllers пуст.");
            return;
        }

        foreach (var controller in targetControllers)
        {
            if (controller != null)
            {
                controller.SetLevel(levelToSet);
            }
            else
            {
                Debug.LogWarning("LevelAvtoActivator: один из targetControllers = null.");
            }
        }
    }
}

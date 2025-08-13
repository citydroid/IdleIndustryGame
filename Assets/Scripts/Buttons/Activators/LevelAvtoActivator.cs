using UnityEngine;

public class LevelAvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("Объект с AvtoMovingController")]
    [SerializeField] private AvtoMovingController targetController;

    [Header("Номер уровня (начиная с 1)")]
    [SerializeField] private int levelToSet = 1;

    public void Execute()
    {
        Debug.Log($"LevelAvtoActivator: Устанавливаем уровень {levelToSet}");

        if (targetController == null)
        {
            Debug.LogWarning("LevelAvtoActivator: targetController не назначен.");
            return;
        }

        targetController.SetLevel(levelToSet);
    }
}
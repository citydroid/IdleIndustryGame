using UnityEngine;
using System.Collections.Generic;

public class ActivatorSpriteWipe : MonoBehaviour, IButtonAction
{
    [Header("Целевые объекты с SpriteWipeController")]
    [SerializeField] private List<SpriteWipeController> targetControllers = new List<SpriteWipeController>();

    [Header("Настройки эффекта протирки")]
    [SerializeField] private float startProgress = 0f;
    [SerializeField] private float endProgress = 1f;
    [SerializeField] private float duration = 1.5f;

    [Header("Направление воспроизведения")]
    [Tooltip("Если true — эффект будет идти вперёд (от startProgress к endProgress), иначе — обратно.")]
    [SerializeField] private bool forward = true;

    public void Execute()
    {
        if (targetControllers == null || targetControllers.Count == 0)
        {
            return;
        }

        foreach (var controller in targetControllers)
        {
            if (controller != null)
            {
                if (forward)
                    controller.StartWipe(startProgress, endProgress, duration);
                else
                    controller.StartWipe(endProgress, startProgress, duration);
            }
            else
            {
                Debug.LogWarning("SpriteWipeActivator: Один из targetControllers не назначен.");
            }
        }
    }
}

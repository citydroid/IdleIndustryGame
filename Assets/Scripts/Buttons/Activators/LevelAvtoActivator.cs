using UnityEngine;
using System.Collections.Generic;

public class LevelAvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("������� � AvtoMovingController")]
    [SerializeField] private List<AvtoMovingController> targetControllers = new List<AvtoMovingController>();

    [Header("����� ������ (������� � 1)")]
    [SerializeField] private int levelToSet = 1;

    public void Execute()
    {
        Debug.Log($"LevelAvtoActivator: ������������� ������� {levelToSet} ��� {targetControllers.Count} ��������");

        if (targetControllers == null || targetControllers.Count == 0)
        {
            Debug.LogWarning("LevelAvtoActivator: ������ targetControllers ����.");
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
                Debug.LogWarning("LevelAvtoActivator: ���� �� targetControllers = null.");
            }
        }
    }
}

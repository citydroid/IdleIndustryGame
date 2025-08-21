using UnityEngine;
using System.Collections.Generic;

public class AvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("������� � AvtoMovingController")]
    [SerializeField] private List<AvtoMovingController> targetControllers = new List<AvtoMovingController>();

    [Header("������ ������� (������� � 0)")]
    [SerializeField] private int prefabIndex = 0;

    public void Execute()
    {
        Debug.Log($"PrefabAvtoActivator: ������������� ������ � �������� {prefabIndex} ��� {targetControllers.Count} ������������");

        if (targetControllers == null || targetControllers.Count == 0)
        {
            Debug.LogWarning("PrefabAvtoActivator: ������ targetControllers ����.");
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
                Debug.LogWarning("PrefabAvtoActivator: ���� �� targetControllers �� ��������.");
            }
        }
    }
}

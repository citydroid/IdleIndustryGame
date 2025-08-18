using UnityEngine;

public class AvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("������ � AvtoMovingController")]
    [SerializeField] private AvtoMovingController targetController;

    [Header("������ ������� (������� � 0)")]
    [SerializeField] private int prefabIndex = 0;

    public void Execute()
    {
        Debug.Log($"PrefabAvtoActivator: ������������� ������ � �������� {prefabIndex}");

        if (targetController == null)
        {
            Debug.LogWarning("PrefabAvtoActivator: targetController �� ��������.");
            return;
        }

        targetController.SetCurrentPrefab(prefabIndex);
    }
}

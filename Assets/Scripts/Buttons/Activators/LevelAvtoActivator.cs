using UnityEngine;

public class LevelAvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("������ � AvtoMovingController")]
    [SerializeField] private AvtoMovingController targetController;

    [Header("����� ������ (������� � 1)")]
    [SerializeField] private int levelToSet = 1;

    public void Execute()
    {
        Debug.Log($"LevelAvtoActivator: ������������� ������� {levelToSet}");

        if (targetController == null)
        {
            Debug.LogWarning("LevelAvtoActivator: targetController �� ��������.");
            return;
        }

        targetController.SetLevel(levelToSet);
    }
}
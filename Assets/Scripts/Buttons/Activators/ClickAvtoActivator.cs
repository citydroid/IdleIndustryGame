using System.Collections.Generic;
using UnityEngine;

public class ClickAvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("������� ������� � ClickToAddMoneyAvto")]
    [SerializeField] private List<ClickToAddMoneyAvto> targetAvto = new List<ClickToAddMoneyAvto>();

    [Header("����� �������� clickReward")]
    [SerializeField] private int newClickReward = 30;

    public void Execute()
    {
        Debug.Log("ClickRewardSetter: Execute ������");

        if (targetAvto == null || targetAvto.Count == 0)
        {
            Debug.LogWarning("ClickRewardSetter: ������ targetPlants ���� ��� �� ��������.");
            return;
        }

        foreach (var plant in targetAvto)
        {
            if (plant != null)
            {
                plant.SetClickReward(newClickReward);
                Debug.Log($"ClickRewardSetter: clickReward ���������� � {newClickReward} ��� {plant.name}");
            }
            else
            {
                Debug.LogWarning("ClickRewardSetter: ���� �� ��������� ������ targetPlants � null.");
            }
        }
    }
}

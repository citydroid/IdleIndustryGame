using System.Collections.Generic;
using UnityEngine;

public class ClickFactoryActivator : MonoBehaviour, IButtonAction
{
    [Header("������� ������� � ClickToAddMoneyPlant")]
    [SerializeField] private List<ClickToAddMoneyPlant> targetPlants = new List<ClickToAddMoneyPlant>();

    [Header("����� �������� clickReward")]
    [SerializeField] private int newClickReward = 20;

    public void Execute()
    {
        Debug.Log("ClickRewardSetter: Execute ������");

        if (targetPlants == null || targetPlants.Count == 0)
        {
            Debug.LogWarning("ClickRewardSetter: ������ targetPlants ���� ��� �� ��������.");
            return;
        }

        foreach (var plant in targetPlants)
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

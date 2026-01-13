using System.Collections.Generic;
using UnityEngine;

public class ClickFactoryActivator : MonoBehaviour, IButtonAction
{
    [Header("Целевые объекты с ClickToAddMoneyPlant")]
    [SerializeField] private List<ClickToAddMoneyPlant> targetPlants = new List<ClickToAddMoneyPlant>();

    [Header("Новое значение clickReward")]
    [SerializeField] private int newClickReward = 20;

    public void Execute()
    {
        Debug.Log("ClickRewardSetter: Execute вызван");

        if (targetPlants == null || targetPlants.Count == 0)
        {
            Debug.LogWarning("ClickRewardSetter: Список targetPlants пуст или не назначен.");
            return;
        }

        for (int i = 0; i < targetPlants.Count; i++)
        {
            var plant = targetPlants[i];

            if (plant != null)
            {
                plant.SetClickReward(newClickReward);

                if (i == 0)
                {
                    plant.PlayClickEffectOnly();
                }
            }
        }
    }

}

using System.Collections.Generic;
using UnityEngine;

public class ClickAvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("Целевые объекты с ClickToAddMoneyAvto")]
    [SerializeField] private List<ClickToAddMoneyAvto> targetAvto = new List<ClickToAddMoneyAvto>();

    [Header("Новое значение clickReward")]
    [SerializeField] private int newClickReward = 30;

    public void Execute()
    {
        Debug.Log("ClickRewardSetter: Execute вызван");

        if (targetAvto == null || targetAvto.Count == 0)
        {
            Debug.LogWarning("ClickRewardSetter: Список targetPlants пуст или не назначен.");
            return;
        }

        foreach (var plant in targetAvto)
        {
            if (plant != null)
            {
                plant.SetClickReward(newClickReward);
                Debug.Log($"ClickRewardSetter: clickReward установлен в {newClickReward} для {plant.name}");
            }
            else
            {
                Debug.LogWarning("ClickRewardSetter: Один из элементов списка targetPlants — null.");
            }
        }
    }
}

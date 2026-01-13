using UnityEngine;
using GamePush;

public class GPRewarded : MonoBehaviour
{
    private MainScript main;   // ссылка на твой MainScript

    private void Awake()
    {
        main = FindObjectOfType<MainScript>();  // ищем на сцене
    }

    private void OnEnable()
    {
        GP_Ads.OnRewardedReward += OnRewarded;
    }

    private void OnDisable()
    {
        GP_Ads.OnRewardedReward -= OnRewarded;
    }

    public void ShowRewarded(string idOrTag = "COINS")
    {
        if (!GP_Init.isReady)
        {
            Debug.LogWarning("GamePush ещё не инициализирован!");
            return;
        }

        if (main == null)
        {
            Debug.LogError(" MainScript не найден на сцене.");
            return;
        }

        Debug.Log($" Показ rewarded рекламы -> Tag: {idOrTag}");

        GP_Ads.ShowRewarded(
            idOrTag: idOrTag,
            onRewardedReward: OnRewarded,
            onRewardedStart: () => Debug.Log("[Rewarded] Показ начался"),
            onRewardedClose: success => Debug.Log("[Rewarded] Закрыта. Success: " + success)
        );
    }

    private void OnRewarded(string idOrTag)
    {
        if (main == null)
        {
            Debug.LogError(" MainScript не найден на сцене.");
            return;
        }

        Debug.Log($"[Rewarded] Выдана награда: {idOrTag}");

        switch (idOrTag)
        {
            case "COINS":
                main.result.TotalValue += 250;
                break;

            case "GEMS":
                main.result.TotalValue += 15;
                break;

            default:
                Debug.LogWarning($"Неизвестный tag награды: {idOrTag}");
                break;
        }

        main.saveSystem.SaveGame();     // сохраняем новые значения
        main.ForceUpdateValues();   // обновляем интерфейс
    }
}

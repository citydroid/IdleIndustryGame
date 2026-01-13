using UnityEngine;
using GamePush;

[RequireComponent(typeof(Collider2D))]
public class ClickAdReward : MonoBehaviour
{
    [Header("Награда за рекламу")]
    [SerializeField] private string rewardTag = "COINS"; // COINS / GEMS / что угодно

    [Header("Анимация при клике")]
    [SerializeField] private float clickScaleFactor = 0.9f;
    [SerializeField] private float animationDuration = 0.1f;

    private Vector3 _originalScale;
    private bool _isAnimating = false;

    private MainScript main;   // ссылка на твою игру (где есть result/increment/save)

    private void Awake()
    {
        _originalScale = transform.localScale;
        main = FindObjectOfType<MainScript>();
    }

    private void OnEnable()
    {
        GP_Ads.OnRewardedReward += OnRewarded;
    }

    private void OnDisable()
    {
        GP_Ads.OnRewardedReward -= OnRewarded;
    }

    private void OnMouseDown()
    {
        if (_isAnimating) return;

        if (!GP_Init.isReady)
        {
            Debug.LogWarning("❌ GamePush Ads не инициализирован!");
            return;
        }

        Debug.Log($"▶ Нажатие! Запуск рекламы с наградой Tag = {rewardTag}");

        GP_Ads.ShowRewarded(
            idOrTag: rewardTag,
            onRewardedReward: OnRewarded,
            onRewardedStart: () => Debug.Log("[Rewarded] Реклама началась"),
            onRewardedClose: success => Debug.Log("[Rewarded] Закрыта. Success: " + success)
        );

        StartCoroutine(ClickAnimation());
    }

    private void OnRewarded(string idOrTag)
    {
        if (main == null)
        {
            Debug.LogError(" MainScript не найден на сцене!");
            return;
        }

        Debug.Log($" Реклама просмотрена! Tag: {idOrTag}");

        switch (idOrTag)
        {
            case "COINS":
                main.result.TotalValue += 25000000000;
                break;

            case "GEMS":
                main.result.TotalValue += 15;
                break;

            default:
                Debug.LogWarning($" Неизвестный rewardTag: {idOrTag}");
                break;
        }

        main.saveSystem.SaveGame();
        main.ForceUpdateValues();
    }

    private System.Collections.IEnumerator ClickAnimation()
    {
        _isAnimating = true;

        // Visually press object
        transform.localScale = _originalScale * clickScaleFactor;

        yield return new WaitForSeconds(animationDuration);

        // Restore
        transform.localScale = _originalScale;
        _isAnimating = false;
    }
}

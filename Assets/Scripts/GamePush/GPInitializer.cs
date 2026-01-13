using UnityEngine;
using GamePush;

public class GPInitializer : MonoBehaviour
{
    // Можно подписаться на событие GP_Init.OnReady
    private void OnEnable()
    {
        GP_Init.OnReady += OnPluginReady;
    }

    // Можно дождаться готовности через await GP_Init.Ready
    private async void Start()
    {
        await GP_Init.Ready;
        OnPluginReady();
    }

    // Можно проверить готовность через GP_Init.isReady
    private void CheckReady()
    {
        if (GP_Init.isReady)
        {
            OnPluginReady();
        }
    }

    private void OnPluginReady()
    {
        Debug.Log("Plugin ready");
    }
}
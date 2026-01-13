using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "TextStandart", menuName = "Localization/TextStandart")]
public class TextStandart : ScriptableObject
{
    [Header("Standard Labels")]
    public LocalizedString priceLabel;
    public LocalizedString requiresLabel;
    public LocalizedString freeLabel;
    public LocalizedString perSecondLabel;

    private static TextStandart _instance;
    public static TextStandart Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<TextStandart>("TextStandart");
                if (_instance == null)
                {
                    Debug.LogError("Не найден TextStandart в папке Resources!");
                }
            }
            return _instance;
        }
    }

    public static string GetPriceLabel()
    {
        if (Instance == null)
            return "Цена:";

        var localized = Instance.priceLabel;
        if (string.IsNullOrEmpty(localized.TableReference.TableCollectionName))
        {
            return "Цена:";
        }

        return localized.GetLocalizedString();
    }

    public static string GetRequiresLabel()
    {
        if (Instance == null)
            return "Надо:";

        var localized = Instance.requiresLabel;
        if (string.IsNullOrEmpty(localized.TableReference.TableCollectionName))
        {
            return "Надо:";
        }

        return localized.GetLocalizedString();
    }

    public static string GetFreeLabel()
    {
        if (Instance == null)
            return "НИЧЕГО";

        var localized = Instance.freeLabel;
        if (string.IsNullOrEmpty(localized.TableReference.TableCollectionName))
        {
            return "НИЧЕГО";
        }

        return localized.GetLocalizedString();
    }
        public static string GetPerSecondLabel()
    {
        if (Instance == null)
            return "в секунду";

        var localized = Instance.perSecondLabel;
        if (string.IsNullOrEmpty(localized.TableReference.TableCollectionName))
        {
            return "в секунду";
        }

        return localized.GetLocalizedString();
    }
}

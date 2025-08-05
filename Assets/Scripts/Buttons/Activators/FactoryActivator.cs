using UnityEngine;

public class FactoryActivator : MonoBehaviour, IButtonAction
{
    [Header("ќбъект, который нужно включить")]
    [SerializeField] private GameObject objectToActivate;

    [Header("ќтключить других детей родител€?")]
    [SerializeField] private bool deactivateSiblings = true;

    public void Execute()
    {
        Debug.Log("FactoryActivator: Execute вызван");

        if (objectToActivate == null)
        {
            Debug.LogWarning("FactoryActivator: objectToActivate не назначен.");
            return;
        }

        if (deactivateSiblings)
        {
            var parent = objectToActivate.transform.parent;
            if (parent != null)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    var child = parent.GetChild(i).gameObject;
                    child.SetActive(child == objectToActivate);
                }
            }
            else
            {
                Debug.LogWarning("FactoryActivator: objectToActivate не имеет родител€ Ч пропускаем деактивацию других.");
            }
        }
        else
        {
            objectToActivate.SetActive(true);
        }
    }
}

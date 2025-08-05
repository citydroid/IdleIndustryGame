using UnityEngine;

public class FactoryActivator : MonoBehaviour, IButtonAction
{
    [Header("������, ������� ����� ��������")]
    [SerializeField] private GameObject objectToActivate;

    [Header("��������� ������ ����� ��������?")]
    [SerializeField] private bool deactivateSiblings = true;

    public void Execute()
    {
        Debug.Log("FactoryActivator: Execute ������");

        if (objectToActivate == null)
        {
            Debug.LogWarning("FactoryActivator: objectToActivate �� ��������.");
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
                Debug.LogWarning("FactoryActivator: objectToActivate �� ����� �������� � ���������� ����������� ������.");
            }
        }
        else
        {
            objectToActivate.SetActive(true);
        }
    }
}

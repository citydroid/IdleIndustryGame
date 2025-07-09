using UnityEngine;

[RequireComponent(typeof(Collider2D))] // ����������� ������� ����������
public class ClickToUnlock : MonoBehaviour
{
    [SerializeField] private UpgradeButton[] targetButtons; // ������ ������, ������� ����� ��������������

    private void OnMouseDown()
    {
        if (targetButtons != null && targetButtons.Length > 0)
        {
            Debug.Log("Object clicked, triggering unlock");
            foreach (var button in targetButtons)
            {
                if (button != null)
                {
                    button.SetExternalTrigger();
                }
                else
                {
                    Debug.LogWarning("One of the target buttons in ClickToUnlock is not set!");
                }
            }
        }
        else
        {
            Debug.LogError("No target buttons assigned in ClickToUnlock!");
        }
    }
}
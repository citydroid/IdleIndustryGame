using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickToUnlock : MonoBehaviour
{
    [SerializeField] private UpgradeButton[] targetButtons; // ������ ������, ������� ����� ��������������
    [SerializeField] private float clickScaleFactor = 0.9f; // ��������� ����������� ������ ��� �����
    [SerializeField] private float animationDuration = 0.1f; // ������������ ��������

    private Vector3 _originalScale;
    private bool _isAnimating = false;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (_isAnimating) return;

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

            // ��������� ��������
            StartCoroutine(ClickAnimation());
        }
        else
        {
            Debug.LogError("No target buttons assigned in ClickToUnlock!");
        }
    }

    private System.Collections.IEnumerator ClickAnimation()
    {
        _isAnimating = true;

        // ��������� ������
        transform.localScale = _originalScale * clickScaleFactor;

        // ���� ��������� �����
        yield return new WaitForSeconds(animationDuration);

        // ���������� �������� ������
        transform.localScale = _originalScale;

        _isAnimating = false;
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickToUnlock : MonoBehaviour
{
    [SerializeField] private UpgradeButton[] targetButtons; // Массив кнопок, которые нужно разблокировать
    [SerializeField] private float clickScaleFactor = 0.9f; // Насколько уменьшается объект при клике
    [SerializeField] private float animationDuration = 0.1f; // Длительность анимации

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

            // Запускаем анимацию
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

        // Уменьшаем объект
        transform.localScale = _originalScale * clickScaleFactor;

        // Ждем указанное время
        yield return new WaitForSeconds(animationDuration);

        // Возвращаем исходный размер
        transform.localScale = _originalScale;

        _isAnimating = false;
    }
}
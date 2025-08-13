using UnityEngine;
using DG.Tweening;

public class TubeActivator : MonoBehaviour, IButtonAction
{
    [Header("Основные настройки")]
    [SerializeField] private GameObject objectToAnimate;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform parentReference; // 🔹 Родительский объект

    [Header("Параметры анимации")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float backSwingDistance = 0.2f;
    [SerializeField] private float overshootDistance = 0.1f;
    [SerializeField] private float overshootDuration = 0.2f;
    [SerializeField] private Ease moveEase = Ease.OutQuad;

    private Vector3 _originalLocalPosition;
    private Sequence _animationSequence;

    private void Awake()
    {
        if (objectToAnimate == null)
            objectToAnimate = gameObject;

        if (parentReference == null)
            Debug.LogWarning("Parent Reference is not assigned in TubeActivator.");

        // 🔹 Запоминаем ЛОКАЛЬНУЮ позицию относительно родителя
        _originalLocalPosition = parentReference != null
            ? parentReference.InverseTransformPoint(objectToAnimate.transform.position)
            : objectToAnimate.transform.localPosition;
    }

    public void Execute()
    {
        if (objectToAnimate == null || endPoint == null || parentReference == null)
        {
            Debug.LogWarning("Не назначены объект, конечная точка или родительский объект!");
            return;
        }

        StartAnimation();
    }

    private void StartAnimation()
    {
        if (_animationSequence != null && _animationSequence.IsActive())
            _animationSequence.Kill();

        _animationSequence = DOTween.Sequence();

        // 🔹 Переводим локальные координаты в мировые, чтобы анимация учитывала смещение родителя
        Vector3 originalWorldPos = parentReference.TransformPoint(_originalLocalPosition);
        Vector3 moveDirection = (endPoint.position - originalWorldPos).normalized;

        _animationSequence.Append(
            objectToAnimate.transform.DOMove(
                originalWorldPos - moveDirection * backSwingDistance,
                moveDuration * 0.3f
            ).SetEase(Ease.OutSine)
        );

        _animationSequence.Append(
            objectToAnimate.transform.DOMove(endPoint.position, moveDuration)
            .SetEase(moveEase)
        );

        _animationSequence.Append(
            objectToAnimate.transform.DOPunchPosition(
                moveDirection * overshootDistance,
                overshootDuration,
                1,
                0.5f
            )
        );
    }

    public void MoveBack()
    {
        if (objectToAnimate == null || parentReference == null) return;

        if (_animationSequence != null && _animationSequence.IsActive())
            _animationSequence.Kill();

        _animationSequence = DOTween.Sequence();

        Vector3 originalWorldPos = parentReference.TransformPoint(_originalLocalPosition);
        Vector3 moveDirection = (originalWorldPos - endPoint.position).normalized;

        _animationSequence.Append(
            objectToAnimate.transform.DOMove(
                endPoint.position - moveDirection * backSwingDistance,
                moveDuration * 0.3f
            ).SetEase(Ease.OutSine)
        );

        _animationSequence.Append(
            objectToAnimate.transform.DOMove(originalWorldPos, moveDuration)
            .SetEase(moveEase)
        );

        _animationSequence.Append(
            objectToAnimate.transform.DOPunchPosition(
                moveDirection * overshootDistance,
                overshootDuration,
                1,
                0.5f
            )
        );
    }

    private void OnDestroy()
    {
        if (_animationSequence != null)
            _animationSequence.Kill();
    }

    private void OnDrawGizmos()
    {
        if (endPoint == null || objectToAnimate == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(endPoint.position, 0.1f);
        Gizmos.DrawLine(objectToAnimate.transform.position, endPoint.position);
    }
}

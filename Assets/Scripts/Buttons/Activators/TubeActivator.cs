using UnityEngine;
using DG.Tweening;

public class TubeActivator : MonoBehaviour, IButtonAction
{
    [Header("�������� ���������")]
    [SerializeField] private GameObject objectToAnimate;
    [SerializeField] private Transform endPoint;

    [Header("��������� ��������")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float backSwingDistance = 0.2f; // ���������� � ������
    [SerializeField] private float overshootDistance = 0.1f;
    [SerializeField] private float overshootDuration = 0.2f;
    [SerializeField] private Ease moveEase = Ease.OutQuad;

    private Vector3 _originalPosition;
    private Sequence _animationSequence;

    private void Awake()
    {
        if (objectToAnimate == null)
            objectToAnimate = gameObject;

        _originalPosition = objectToAnimate.transform.position;
    }

    public void Execute()
    {
        if (objectToAnimate == null || endPoint == null)
        {
            Debug.LogWarning("�� ��������� ������ ��� �������� �����!");
            return;
        }

        StartAnimation();
    }

    private void StartAnimation()
    {
        // ��������� ���������� ��������
        if (_animationSequence != null && _animationSequence.IsActive())
        {
            _animationSequence.Kill();
        }

        _animationSequence = DOTween.Sequence();

        // ����������� ��������
        Vector3 moveDirection = (endPoint.position - _originalPosition).normalized;

        // 1. ��������� ���������� �����
        _animationSequence.Append(
            objectToAnimate.transform.DOMove(
                _originalPosition - moveDirection * backSwingDistance,
                moveDuration * 0.3f)
            .SetEase(Ease.OutSine)
        );

        // 2. �������� �������� � ����
        _animationSequence.Append(
            objectToAnimate.transform.DOMove(endPoint.position, moveDuration)
            .SetEase(moveEase)
        );

        // 3. ��������� ������ � �����
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
        if (objectToAnimate == null) return;

        if (_animationSequence != null && _animationSequence.IsActive())
        {
            _animationSequence.Kill();
        }

        _animationSequence = DOTween.Sequence();

        // ����������� �������� �����
        Vector3 moveDirection = (_originalPosition - endPoint.position).normalized;

        // 1. ��������� ���������� �����
        _animationSequence.Append(
            objectToAnimate.transform.DOMove(
                endPoint.position - moveDirection * backSwingDistance,
                moveDuration * 0.3f)
            .SetEase(Ease.OutSine)
        );

        // 2. �������� �������� � �������� �������
        _animationSequence.Append(
            objectToAnimate.transform.DOMove(_originalPosition, moveDuration)
            .SetEase(moveEase)
        );

        // 3. ��������� ������ � �����
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
        {
            _animationSequence.Kill();
        }
    }

    private void OnDrawGizmos()
    {
        if (endPoint == null || objectToAnimate == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(endPoint.position, 0.1f);
        Gizmos.DrawLine(objectToAnimate.transform.position, endPoint.position);
    }
}
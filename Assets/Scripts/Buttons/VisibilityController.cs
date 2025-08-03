using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    [SerializeField] private GameObject[] objects; // ������ ���� ��������

    private void Start()
    {
        // ������������� - ��� ������� ��������� (��� ��������� �� ���������)
        SetAllObjectsInactive();
        objects[0].SetActive(true);
    }

    // ��������� ��� �������
    private void SetAllObjectsInactive()
    {
        foreach (var obj in objects)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    // �������� ���������� ������ �� ������� (��������� ���������)
    public void ActivateObject(int index)
    {
        if (index < 0 || index >= objects.Length) return;

        SetAllObjectsInactive();
        objects[index].SetActive(true);
    }

    // �������������� ������� - ������ �������� ������, �� �������� ������
    public void ShowObject(int index)
    {
        if (index < 0 || index >= objects.Length) return;
        if (objects[index] != null) objects[index].SetActive(true);
    }
}
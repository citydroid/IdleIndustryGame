using UnityEngine;

public class AvtoPrefabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] levels; // Level0, Level1, Level2...

    /// <summary>
    /// �������� ��������� ������� �� �������, �������� ��� ���������
    /// </summary>
    public void ActivateLevel(int index)
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogWarning("LevelSwitcher: ������ levels ����.");
            return;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] != null)
                levels[i].SetActive(i == index);
        }
    }

#if UNITY_EDITOR
    // ����� ������������� ��������� ������ ��������� ��������� � ���������
    private void OnValidate()
    {
        if (levels == null || levels.Length == 0)
        {
            int childCount = transform.childCount;
            levels = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                levels[i] = transform.GetChild(i).gameObject;
            }
        }
    }
#endif
}

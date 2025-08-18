using UnityEngine;

public class AvtoPrefabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;

    public void ActivateLevel(int index)
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogWarning("LevelSwitcher: массив levels пуст.");
            return;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] != null)
                levels[i].SetActive(i == index);
        }
    }

    public GameObject GetActiveChild()
    {
        if (levels == null) return null;

        foreach (var lvl in levels)
        {
            if (lvl != null && lvl.activeSelf)
                return lvl;
        }
        return null;
    }

#if UNITY_EDITOR
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

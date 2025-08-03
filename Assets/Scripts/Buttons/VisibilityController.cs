using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    [SerializeField] private GameObject[] objects; // Массив всех объектов

    private void Start()
    {
        // Инициализация - все объекты выключены (или настройте по умолчанию)
        SetAllObjectsInactive();
        objects[0].SetActive(true);
    }

    // Выключает все объекты
    private void SetAllObjectsInactive()
    {
        foreach (var obj in objects)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    // Включает конкретный объект по индексу (остальные выключает)
    public void ActivateObject(int index)
    {
        if (index < 0 || index >= objects.Length) return;

        SetAllObjectsInactive();
        objects[index].SetActive(true);
    }

    // Альтернативный вариант - просто включает объект, не выключая другие
    public void ShowObject(int index)
    {
        if (index < 0 || index >= objects.Length) return;
        if (objects[index] != null) objects[index].SetActive(true);
    }
}
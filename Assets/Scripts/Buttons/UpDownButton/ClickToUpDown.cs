using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickToUpDown : MonoBehaviour
{
    [Header("“ип кнопки")]
    [Tooltip("¬ыберите действие: MoveDown Ч опуститьс€ ниже, MoveUp Ч подн€тьс€ выше.")]
    [SerializeField] private ButtonType buttonType = ButtonType.MoveDown;

    [Header("—сылка на контроллер перемещени€ уровней")]
    [SerializeField] private DragAndScrollController scrollController;

    [SerializeField] private ArrowsOn activateArrowFirst;
    [SerializeField] private ArrowsOn activateArrowSecond;

    private int maxLevel = 0;
    public enum ButtonType
    {
        MoveDown, 
        MoveUp    
    }

    private void Awake()
    {
        if (scrollController == null)
        {
            scrollController = FindObjectOfType<DragAndScrollController>();
        }
    }

    private void OnMouseDown()
    {
        if (scrollController == null) return;

        switch (buttonType)
        {
            case ButtonType.MoveDown: 
                MoveDown();           
                break;

            case ButtonType.MoveUp:   
                MoveUp();             
                break;
        }
    }

    private void MoveDown()
    {
        int current = scrollController.GetCurrentLevel();
        if (current > 0)
        {
            scrollController.ScrollToLevel(current - 1);

            activateArrowFirst.UpdateArrowUp(current - 1);

            if (current - 1 != maxLevel)
            {
                activateArrowSecond.UpdateArrowDownOn();
            }
            else
            {
                activateArrowSecond.UpdateArrowDownMax();
            }
        } 
    }

    private void MoveUp() 
    {
        int current = scrollController.GetCurrentLevel();
        if (current + 1 <= maxLevel)
        {
            scrollController.GoToNextLevel();

            if (current + 1 > 0)
                activateArrowFirst.UpdateArrowUp(current + 1);

            if (current + 1 != maxLevel)
            {
                activateArrowSecond.UpdateArrowDownOn();
            }
            else
            {
                activateArrowSecond.UpdateArrowDownMax();
            }
        }
    }



    public void MaxLevelActivator(int _maxLevel)
    {
        maxLevel = _maxLevel;

    }
}

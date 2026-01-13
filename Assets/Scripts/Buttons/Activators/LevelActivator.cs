using UnityEngine;

public class LevelActivator : MonoBehaviour, IButtonAction
{
    [SerializeField] private DragAndScrollController scrollController;
    [SerializeField] private ClickToUpDown buttonUp;
    [SerializeField] private ClickToUpDown buttonDown;
    [SerializeField] private ArrowsOn arrowUp;
    [SerializeField] private ArrowsOn arrowDown;
    [SerializeField] private int levelToGo = 1;

    public void Execute()
    {
        if (scrollController != null)
        {
            scrollController.ScrollToLevel(levelToGo);
            buttonUp.MaxLevelActivator(levelToGo);
            buttonDown.MaxLevelActivator(levelToGo);
            arrowUp.UpdateArrowUp(levelToGo);
            arrowDown.UpdateArrowDownMax();
        }
    }
}

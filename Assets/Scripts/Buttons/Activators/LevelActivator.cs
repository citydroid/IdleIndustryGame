using UnityEngine;

public class LevelActivator : MonoBehaviour, IButtonAction
{
    [SerializeField] private DragAndScrollController scrollController;
    [SerializeField] private int levelToGo = 1;

    public void Execute()
    {
        if (scrollController != null)
        {
            scrollController.ScrollToLevel(levelToGo);
        }
    }
}

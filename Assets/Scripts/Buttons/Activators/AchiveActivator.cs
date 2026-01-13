
using UnityEngine;

public class AchiveActivator : MonoBehaviour, IButtonAction
{
    [SerializeField] private GameObject achiveOn;
    [SerializeField] private FinalButtonOn finalButton;
    [SerializeField] private int numb = 1;

    public void Execute()
    {
        if (achiveOn != null)
        {
            achiveOn.SetActive(true);
            finalButton.SetCurrentValue(numb);
        }
    }
}

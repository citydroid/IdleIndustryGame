using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "MainButtonObjects", menuName = "Game/Increment Settings")]
public class IncrementSettings : ScriptableObject
{
    [System.Serializable]
    public class ButtonData
    {
        public LocalizedString buttonName; 
        public int level = 1; 
        public int incrementValue;
        public int incrementCoefficient;
        public int cost;
        public int costCoefficient;
        public Sprite activeSprite;
        public Sprite inactiveSprite;
    }

    public List<ButtonData> buttonsData = new List<ButtonData>();

    public ButtonData GetButtonData(int index)
    {
        if (index >= 0 && index < buttonsData.Count)
            return buttonsData[index];

        Debug.LogError($"Button data with index {index} not found!");
        return default;
    }
}
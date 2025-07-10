using UnityEngine;
using TMPro;
using System.Globalization;

public class MainScript : MonoBehaviour
{
    public TextMeshPro resultText;    
    public TextMeshPro incrementText;
    public TextMeshPro infoTextName;
    public TextMeshPro infoTextCost;
    public TextMeshPro infoTextCondition;

    public Increment increment;
    public Result result;

    private void Start()
    {
        increment = new Increment();
        result = new Result(this);
        InvokeRepeating(nameof(UpdateUI), 0, 0.1f);
    }

    private void UpdateUI()
    {
        string formattedResult = FormatCost(result.TotalValue);

        if (resultText != null)
            resultText.text = formattedResult;

        if (incrementText != null)
            incrementText.text = "+ " + increment.Value.ToString();
/*
        if (infoText != null)
            infoText.text = "Рост: " + increment.Value.ToString();
*/
    }
    private string FormatCost(int cost)
    {
        // Форматируем число с разделителем тысяч (точкой)
        return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", cost).Replace(",", ".");
    }
}
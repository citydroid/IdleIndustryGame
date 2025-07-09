using UnityEngine;
using TMPro;
using System.Globalization;

public class MainScript : MonoBehaviour
{
    public TextMeshPro resultText;    
    public TextMeshPro incrementText;
    public TextMeshPro infoText;

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
        if (resultText != null)
            resultText.text = string.Format("{0:#,##0}", result.TotalValue).Replace(",", ".");

        if (incrementText != null)
            incrementText.text = "Рост: " + increment.Value.ToString();

        if (infoText != null)
            infoText.text = "Рост: " + increment.Value.ToString();
    }
}
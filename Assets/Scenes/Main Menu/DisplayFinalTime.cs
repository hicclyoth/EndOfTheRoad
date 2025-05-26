using TMPro;
using UnityEngine;

public class DisplayFinalTime : MonoBehaviour
{
    private TextMeshProUGUI timeText;

    void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();

        if (GameTimer.Instance != null)
        {
            timeText.text = "Final Time: " + GameTimer.Instance.GetFormattedTime();
        }
        else
        {
            timeText.text = "No Time Recorded";
        }
    }
}

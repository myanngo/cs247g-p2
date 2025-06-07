using UnityEngine;

public class Cutscene : MonoBehaviour
{
    // Drag the Canvas GameObject into this field in the Inspector
    public GameObject canvasToShow;

    // Method to be triggered from the Timeline via a Signal
    public void ShowCanvas()
    {
        canvasToShow.SetActive(true);
    }
}

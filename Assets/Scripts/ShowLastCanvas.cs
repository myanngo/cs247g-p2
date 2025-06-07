using UnityEngine;

public class ShowLastCanvas : MonoBehaviour
{
    public GameObject canvasToShow;

    public void ShowCanvas()
    {
        canvasToShow.SetActive(true);
    }
}

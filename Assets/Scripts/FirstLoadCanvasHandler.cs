using UnityEngine;

public class FirstLoadCanvasHandler : MonoBehaviour
{
    public GameObject canvasPanel;
    private static bool hasShownPanel = false;

    void Start()
    {
        if (!hasShownPanel)
        {
            hasShownPanel = true;
            canvasPanel.SetActive(true); // show the panel
        }
        else
        {
            canvasPanel.SetActive(false); // hide it on returns
        }
    }
}

using UnityEngine;

public class WalkToScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Name of the scene to transition to
    [SerializeField] private float transitionDelay = 0f; // Optional delay before fade starts

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure your player GameObject has the "Player" tag
        {
            if (FadeManager.Instance != null)
            {
                FadeManager.Instance.FadeToSceneWithDelay(sceneToLoad, transitionDelay);
            }
            else
            {
                Debug.LogWarning("FadeManager.Instance is null. Make sure a FadeManager is in the scene.");
            }
        }
    }
}

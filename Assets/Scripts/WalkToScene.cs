using UnityEngine;

public class WalkToScene : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private string spawnPointInTargetScene;
    [SerializeField] private float transitionDelay = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Globals.LastScene = targetScene;
            Globals.SpawnPointID = spawnPointInTargetScene;

            FadeManager.Instance.FadeToSceneWithDelay(targetScene, transitionDelay);
        }
    }
}

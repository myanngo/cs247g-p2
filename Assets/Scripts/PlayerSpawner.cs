using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject LilyOfValley;
    [SerializeField] private GameObject Bottle;
    [SerializeField] private GameObject Honey;
    void Start()
    {
        if (Globals.SpawnPointID == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // Support scenes with no player

        // Find matching spawn point
        SpawnPoint[] points = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (var point in points)
        {
            if (point.spawnID == Globals.SpawnPointID)
            {
                player.transform.position = point.transform.position;
                break;
            }
        }

        // spawn contextual object based on last scene
        if (Globals.LastScene == "FlowerPuzzle")
        {
            if (LilyOfValley != null)
            {
                LilyOfValley.SetActive(true);
            }
        }

        // spawn contextual object based on last scene
        if (Globals.LastScene == "PuzzleInterface")
        {
            if (Bottle != null)
            {
                Bottle.SetActive(true);
            }
        }

        // spawn contextual object based on last scene
        if (Globals.LastScene == "VerticalPlatformer")
        {
            if (Honey != null)
            {
                Honey.SetActive(true);
            }
        }

        // Clear after use
        Globals.SpawnPointID = null;
    }
}

using UnityEngine;

public class FlowerLevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Sprite[] flowerSprites; // Array to hold the 14 different flower sprites
    
    [SerializeField]
    private float flowerScale = 2f; // Scale factor for the flowers
    private float flowerScaleBig = 4f;
    
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (flowerSprites == null || flowerSprites.Length == 0)
        {
            Debug.LogError("No flower sprites assigned to FlowerLevelGenerator!");
            return;
        }

        // Calculate screen boundaries in world coordinates
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found!");
            return;
        }

        // Get the screen bounds in world coordinates
        float verticalSize = mainCamera.orthographicSize;
        float horizontalSize = verticalSize * mainCamera.aspect;

        // Set the boundaries with a small margin (0.9f) to keep flowers slightly inside the view
        minX = -horizontalSize * 0.9f;
        maxX = horizontalSize * 0.9f;
        minY = -verticalSize * 0.9f;
        maxY = verticalSize * 0.9f;

        // For each sprite type
        for (int i = 0; i < flowerSprites.Length; i++)
        {
            // Spawn 10 instances of each sprite
            for (int j = 0; j < 20; j++)
            {
                SpawnFlower(flowerSprites[i], i <= 6 ? flowerScale : flowerScaleBig);
            }
        }
    }

    private void SpawnFlower(Sprite flowerSprite, float scale)
    {
        // Create a new GameObject
        GameObject flower = new GameObject("Flower");
        
        // Add SpriteRenderer component
        SpriteRenderer spriteRenderer = flower.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = flowerSprite;
        
        // Set random position
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        flower.transform.position = new Vector3(randomX, randomY, 0);
        
        // Set the scale
        flower.transform.localScale = Vector3.one * scale;
        
        // Make it a child of this generator
        flower.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

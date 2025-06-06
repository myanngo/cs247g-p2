using UnityEngine;

public class FlashlightEffect : MonoBehaviour
{
    [SerializeField] private Material flashlightMaterial;
    [SerializeField] private float flashlightSize = 2f;
    [SerializeField] [Range(0f, 1f)] private float darkness = 0.85f;
    [SerializeField] private GameObject lilyOfValley;

    private Camera mainCamera;
    private static readonly int MousePosID = Shader.PropertyToID("_MousePosition");
    private static readonly int FlashlightSizeID = Shader.PropertyToID("_FlashlightSize");
    private static readonly int DarknessID = Shader.PropertyToID("_Darkness");
    private GameObject quad;
    private bool isDarknessActive = true;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Make sure your camera has the 'MainCamera' tag.");
            return;
        }

        // Ensure camera is orthographic
        if (!mainCamera.orthographic)
        {
            Debug.LogWarning("Camera should be orthographic for best results!");
        }

        if (lilyOfValley != null)
        {
            // Add collider to lily-of-the-valley if it doesn't have one
            if (lilyOfValley.GetComponent<Collider2D>() == null)
            {
                BoxCollider2D collider = lilyOfValley.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }
        }
        else
        {
            Debug.LogWarning("Lily of the Valley not assigned!");
        }
        
        CreateOverlay();
    }

    private void CreateOverlay()
    {
        // Create a full-screen quad
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = "DarknessOverlay";
        
        // Position it in front of everything
        float zPosition = mainCamera.transform.position.z + 1f;
        quad.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, zPosition);
        quad.transform.rotation = Quaternion.identity;
        
        // Calculate the proper size to cover the entire view
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        quad.transform.localScale = new Vector3(width, height, 1) * 1.2f; // 20% larger to ensure coverage
        
        // Set up the material
        var renderer = quad.GetComponent<Renderer>();
        renderer.material = flashlightMaterial;
        renderer.sortingOrder = 1000; // Ensure it renders on top
        
        // Initial material setup
        flashlightMaterial.SetFloat(FlashlightSizeID, flashlightSize);
        flashlightMaterial.SetFloat(DarknessID, darkness);

        // Remove the mesh collider as we don't need it
        DestroyImmediate(quad.GetComponent<MeshCollider>());
    }

    private void LateUpdate()
    {
        if (mainCamera == null || quad == null) return;

        // Only update if darkness is active
        if (!isDarknessActive)
        {
            if (quad.activeSelf)
            {
                quad.SetActive(false);
            }
            return;
        }

        // Update quad position to follow camera
        Vector3 cameraPos = mainCamera.transform.position;
        quad.transform.position = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z + 1f);
        
        // Calculate proper size based on current camera
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        quad.transform.localScale = new Vector3(width, height, 1) * 1.2f;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        
        // Check for mouse click on lily-of-the-valley
        if (Input.GetMouseButtonDown(0) && lilyOfValley != null)
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(mousePos);
            
            Collider2D collider = lilyOfValley.GetComponent<Collider2D>();
            if (collider != null && collider.OverlapPoint(worldPoint))
            {
                isDarknessActive = false;
                FadeManager.Instance.FadeToSceneWithDelay("Final_Map", 0F);
                return;
            }
        }

        // Update flashlight position
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        flashlightMaterial.SetVector(MousePosID, new Vector4(worldPos.x, worldPos.y, 0, 0));
    }

    private void OnValidate()
    {
        // Update material properties when changed in inspector
        if (flashlightMaterial != null)
        {
            flashlightMaterial.SetFloat(FlashlightSizeID, flashlightSize);
            flashlightMaterial.SetFloat(DarknessID, darkness);
        }
    }

    // Method to reset darkness if needed
    public void ResetDarkness()
    {
        isDarknessActive = true;
        if (quad != null)
            quad.SetActive(true);
    }
} 
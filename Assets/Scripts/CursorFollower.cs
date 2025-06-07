using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float smoothSpeed = 10f;  // How smoothly the sprite follows the cursor
    [SerializeField] private Vector3 offset;           // Offset from the cursor position
    [SerializeField] private bool flipSpriteWithMovement = true;  // Whether to flip the sprite based on movement direction

    [Header("Animation Settings")]
    [SerializeField] private bool useAnimation = false;  // Toggle animation support

    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 lastPosition;
    private bool isMoving;
    private bool gameWindowFocused = false;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found!");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject!");
        }

        // Only try to get the animator if animation is enabled
        if (useAnimation)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animation is enabled but no Animator component found. Add an Animator component to use animations.");
            }
        }

        lastPosition = transform.position;
        
        // Initial cursor hide attempt
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // Check for any mouse click to ensure cursor is hidden
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!gameWindowFocused)
            {
                gameWindowFocused = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        // Get mouse position in world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePos) + offset;

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Check if moving (only if we're using animation or need sprite flipping)
        if (useAnimation || flipSpriteWithMovement)
        {
            float movementThreshold = 0.01f;
            isMoving = Vector3.Distance(transform.position, lastPosition) > movementThreshold;
        }

        // Update animation if enabled and we have an animator
        if (useAnimation && animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }

        // Flip sprite based on movement direction if enabled
        if (flipSpriteWithMovement && spriteRenderer != null)
        {
            Vector3 movement = transform.position - lastPosition;
            if (Mathf.Abs(movement.x) > 0.01f)  // Small threshold to prevent flipping when barely moving
            {
                spriteRenderer.flipX = movement.x < 0;
            }
        }

        lastPosition = transform.position;

        // Double-check cursor state
        if (gameWindowFocused && Cursor.visible)
        {
            Cursor.visible = false;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            gameWindowFocused = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnDestroy()
    {
        // Show the cursor when this component is destroyed
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Helper method to enable/disable animation at runtime if needed
    public void SetAnimationEnabled(bool enabled)
    {
        useAnimation = enabled;
        if (enabled && animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
} 
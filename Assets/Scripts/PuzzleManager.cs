using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public Transform solveArea;
    public Transform scatterRegion; // Region where pieces will be scattered
    public GameObject silhouettePrefab; // Prefab for the silhouette guide

    public Sprite[] glassSprites; // Array to hold the sprites from the "glass" set
    public Sprite solvedSprite; // Sprite to show when the puzzle is solved
    public Sprite continueButtonSprite; // Sprite for the continue button
    private float puzzleScale = 1.2f; // Shared scale for both pieces and silhouette
    private List<GameObject> puzzlePieces = new List<GameObject>();
    private GameObject draggingPiece = null;
    private Vector3 offset;
    private Dictionary<GameObject, Vector3> solutionCoordinates = new Dictionary<GameObject, Vector3>();
    private bool done;

    void Start()
    {
        CreateSilhouette();
        CreatePuzzle();
        done = false;
        InitializeSolutionCoordinates();
        Scatter();
    }

    // Function to initialize the solution coordinates
    void InitializeSolutionCoordinates()
    {

        solutionCoordinates[puzzlePieces[0]] = new Vector3(0.1436154f, 2.401809f, 0f);
        solutionCoordinates[puzzlePieces[1]] = new Vector3(0.7018303f, 0.9369204f, 0f);
        solutionCoordinates[puzzlePieces[2]] = new Vector3(-0.5270975f, 1.981371f, 0f);
        solutionCoordinates[puzzlePieces[3]] = new Vector3(-0.4972994f, 0.7443328f, 0f);
        solutionCoordinates[puzzlePieces[4]] = new Vector3(0.204105f, 1.726616f, 0f);
        solutionCoordinates[puzzlePieces[5]] = new Vector3(-0.8273472f, -0.3345942f, 0f);
        solutionCoordinates[puzzlePieces[6]] = new Vector3(-0.7555011f, -2.342153f, 0f);
        solutionCoordinates[puzzlePieces[7]] = new Vector3(0.2256437f, -2.252764f, 0f);
        solutionCoordinates[puzzlePieces[8]] = new Vector3(0.1480241f, -0.8830826f, 0f);
        solutionCoordinates[puzzlePieces[9]] = new Vector3(1.022338f, -0.3801977f, 0f);
        solutionCoordinates[puzzlePieces[10]] = new Vector3(0.6451153f, -2.162156f, 0f);
        solutionCoordinates[puzzlePieces[11]] = new Vector3(-0.5242504f, -1.397804f, 0f);
        solutionCoordinates[puzzlePieces[12]] = new Vector3(-0.03084381f, 0.2468064f, 0f);
        solutionCoordinates[puzzlePieces[13]] = new Vector3(0.6697233f, -1.387977f, 0f);


        // solutionCoordinates[puzzlePieces[0]] = new Vector3(0.1365065f, 2.415168f, 0f);
        // solutionCoordinates[puzzlePieces[1]] = new Vector3(0.6960804f, 0.9533092f, 0f);
        // solutionCoordinates[puzzlePieces[2]] = new Vector3(-0.8231218f, -0.3432156f, 0f);
        // solutionCoordinates[puzzlePieces[3]] = new Vector3(-0.520515f, 1.940631f, 0f);
        // solutionCoordinates[puzzlePieces[4]] = new Vector3(0.1624695f, 1.717028f, 0f);
        // solutionCoordinates[puzzlePieces[5]] = new Vector3(-0.5485473f, 0.680092f, 0f);
        // solutionCoordinates[puzzlePieces[6]] = new Vector3(-0.01293063f, 0.2254836f, 0f);
        // solutionCoordinates[puzzlePieces[7]] = new Vector3(-0.7562358f, -2.315372f, 0f);
        // solutionCoordinates[puzzlePieces[8]] = new Vector3(0.1355052f, -0.9252224f, 0f);
        // solutionCoordinates[puzzlePieces[9]] = new Vector3(1.034477f, -0.3207911f, 0f);
        // solutionCoordinates[puzzlePieces[10]] = new Vector3(0.6777353f, -1.325979f, 0f);
        // solutionCoordinates[puzzlePieces[11]] = new Vector3(0.2580201f, -2.245838f, 0f);
        // solutionCoordinates[puzzlePieces[12]] = new Vector3(-0.5379066f, -1.401858f, 0f);
        // solutionCoordinates[puzzlePieces[13]] = new Vector3(0.6588899f, -2.120884f, 0f);
    }

    // Function to check if the puzzle is solved
    bool IsPuzzleSolved()
    {
        if (puzzlePieces.Count == 0) return false;

        // Calculate the offset of the first piece
        Vector3 firstPieceOffset = puzzlePieces[0].transform.position - solutionCoordinates[puzzlePieces[0]];
        float tolerance = puzzleScale * 0.8f; // Scale the tolerance with the puzzle size

        // Check if all pieces have the same offset (within tolerance)
        foreach (GameObject piece in puzzlePieces)
        {
            Vector3 currentOffset = piece.transform.position - solutionCoordinates[piece];
            float distance = (currentOffset - firstPieceOffset).magnitude;
                        
            if (distance > tolerance)
            {
                return false; // Puzzle is not solved
            }
        }

        Debug.Log("All pieces within tolerance - Puzzle Solved!");
        return true; // Puzzle is solved
    }

    // Function to show the completed puzzle
    void ShowCompletedPuzzle()
    {
        Debug.Log("Showing completed puzzle!");
        // Deactivate all puzzle pieces
        Vector3 newPos = solveArea.position;
        foreach (GameObject piece in puzzlePieces)
        {
            piece.SetActive(false);
        }
 
        // Create a new GameObject for the completed puzzle
        GameObject completedPuzzle = new GameObject("CompletedPuzzle");
        completedPuzzle.transform.position = newPos;
        completedPuzzle.transform.SetParent(solveArea);
        completedPuzzle.transform.localPosition = Vector3.zero;

        // Add a SpriteRenderer and assign the solvedSprite
        SpriteRenderer renderer = completedPuzzle.AddComponent<SpriteRenderer>();
        renderer.sprite = solvedSprite;
        renderer.sortingOrder = 2; // On top of everything
        completedPuzzle.transform.localScale = new Vector3(puzzleScale, puzzleScale, 1f);
        
        Debug.Log("Completed puzzle sprite created and displayed");

        // Create continue button
        if (continueButtonSprite != null)
        {
            GameObject continueButton = new GameObject("ContinueButton");
            continueButton.transform.SetParent(transform); // Parent to PuzzleManager
            
            // Add SpriteRenderer for the button
            SpriteRenderer buttonRenderer = continueButton.AddComponent<SpriteRenderer>();
            buttonRenderer.sprite = continueButtonSprite;
            buttonRenderer.sortingOrder = 3; // Above everything else
            
            // Add BoxCollider2D for click detection
            BoxCollider2D buttonCollider = continueButton.AddComponent<BoxCollider2D>();
            
            // Position the button at the top of the screen
            Camera mainCamera = Camera.main;
            float buttonScale = 1f; // Adjust this value to change button size
            continueButton.transform.localScale = new Vector3(buttonScale, buttonScale, 1f);
            
            // Position button near top-center of screen
            if (mainCamera != null)
            {
                float screenHeight = mainCamera.orthographicSize * 2;
                float yPosition = mainCamera.transform.position.y - (screenHeight * 0.35f); // 40% from top
                continueButton.transform.position = new Vector3(0f, yPosition, 0f);
            }
            else
            {
                // Fallback position if no camera is found
                continueButton.transform.position = new Vector3(0f, 4f, 0f);
            }
        }
    }

    void CreatePuzzle()
    {
        foreach (Sprite sprite in glassSprites)
        {
            GameObject piece = Instantiate(puzzlePiecePrefab, solveArea);
            piece.name = "Piece " + sprite.name;
            SpriteRenderer renderer = piece.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = sprite;
                renderer.sortingOrder = 2; // Pieces on top
                // Scale the piece using shared scale
                piece.transform.localScale = new Vector3(puzzleScale, puzzleScale, 1f);
            }
            puzzlePieces.Add(piece);
        }
    }

    void Scatter()
    {
        Debug.Log("Scattering pieces...");
        
        if (scatterRegion == null)
        {
            Debug.LogError("Scatter region not assigned!");
            return;
        }

        // Get the actual rendered bounds of the scatter region
        SpriteRenderer scatterRenderer = scatterRegion.GetComponent<SpriteRenderer>();
        if (scatterRenderer == null)
        {
            Debug.LogError("Scatter region needs a SpriteRenderer!");
            return;
        }

        Bounds scatterBounds = scatterRenderer.bounds;
        
        // Calculate the actual bounds of the scatter region
        float minX = scatterBounds.min.x;
        float maxX = scatterBounds.max.x;
        float minY = scatterBounds.min.y;
        float maxY = scatterBounds.max.y;
        
        float padding = 0.5f; // Add padding to keep pieces inside the region

        foreach (GameObject piece in puzzlePieces)
        {
            SpriteRenderer pieceRenderer = piece.GetComponent<SpriteRenderer>();
            // Account for the piece's scaled size
            float pieceWidth = pieceRenderer.bounds.size.x;
            float pieceHeight = pieceRenderer.bounds.size.y;

            // Calculate random position within the region bounds, accounting for piece size and padding
            float x = Random.Range(
                minX + pieceWidth/2 + padding, 
                maxX - pieceWidth/2 - padding
            );
            float y = Random.Range(
                minY + pieceHeight/2 + padding, 
                maxY - pieceHeight/2 - padding
            );

            piece.transform.position = new Vector3(x, y, 0);
        }
    }

    void CreateSilhouette()
    {
        Debug.Log("Creating silhouette...");
        if (silhouettePrefab != null && solvedSprite != null && solveArea != null)
        {
            GameObject silhouette = new GameObject("Silhouette");
            SpriteRenderer renderer = silhouette.AddComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Debug.Log("Setting up silhouette renderer...");
                renderer.sprite = solvedSprite;
                // Make it dark and semi-transparent for a silhouette effect, with no background
                renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.3f); // Darker gray with 30% opacity
                renderer.sortingOrder = 1; // Silhouette between solve area and pieces
                renderer.maskInteraction = SpriteMaskInteraction.None;
                renderer.spriteSortPoint = SpriteSortPoint.Center;

                // First set the base scale to match puzzle pieces
                silhouette.transform.SetParent(solveArea);
                silhouette.transform.localPosition = Vector3.zero;
                silhouette.transform.localScale = new Vector3(puzzleScale, puzzleScale, 1f);

                // Get the actual rendered bounds of the solve area
                SpriteRenderer solveAreaRenderer = solveArea.GetComponent<SpriteRenderer>();
                if (solveAreaRenderer == null)
                {
                    Debug.LogError("Solve area needs a SpriteRenderer!");
                    return;
                }

                Bounds solveAreaBounds = solveAreaRenderer.bounds;
                float areaWidth = solveAreaBounds.size.x;
                float areaHeight = solveAreaBounds.size.y;

                // Now check if silhouette fits within solve area bounds
                float silhouetteWidth = renderer.bounds.size.x;
                float silhouetteHeight = renderer.bounds.size.y;

                // If silhouette is too big, scale it down proportionally to fit
                if (silhouetteWidth > areaWidth || silhouetteHeight > areaHeight)
                {
                    float widthRatio = areaWidth / silhouetteWidth;
                    float heightRatio = areaHeight / silhouetteHeight;
                    float fitScale = Mathf.Min(widthRatio, heightRatio);
                    silhouette.transform.localScale *= fitScale;
                }

                Debug.Log($"Silhouette created in solve area: {silhouette.transform.position}, " +
                         $"final scale: {silhouette.transform.localScale}, " +
                         $"silhouette size: {renderer.bounds.size}, " +
                         $"solve area size: {solveAreaBounds.size}");
            }
            else
            {
                Debug.LogError("Failed to add SpriteRenderer to silhouette!");
            }
        }
        else
        {
            Debug.LogError($"Missing components - silhouettePrefab: {silhouettePrefab != null}, solvedSprite: {solvedSprite != null}, solveArea: {solveArea != null}");
        }
    }

    public void ResetPuzzle()
    {
        foreach (GameObject piece in puzzlePieces)
        {
            piece.transform.localPosition = Vector3.zero;
        }
    }

    void Update() {
        if (IsPuzzleSolved())
        {
            Debug.Log("Puzzle Solved!");
            if (!done) ShowCompletedPuzzle();
            done = true;
            
            // Check for continue button click when puzzle is solved
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit && hit.collider != null && hit.collider.gameObject.name == "ContinueButton")
                {
                    Debug.Log("Continue button clicked!");
                    FadeManager.Instance.FadeToSceneWithDelay("Final_Map", 0F);
                }
            }
            return; // Don't process other input when puzzle is solved
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit) {
                offset = hit.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
                draggingPiece = hit.transform.gameObject;
            }
        }

        if (draggingPiece && Input.GetMouseButtonUp(0)) {
            draggingPiece.transform.position += Vector3.forward;
            draggingPiece = null;
            offset = Vector3.zero;
        }

        if (draggingPiece) {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition += offset;
            draggingPiece.transform.position = newPosition;
        }
    }
}

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public Transform puzzleArea;

    public Sprite[] glassSprites; // Array to hold the sprites from the "glass" set
    public Sprite solvedSprite; // Sprite to show when the puzzle is solved


    private List<GameObject> puzzlePieces = new List<GameObject>();
    private GameObject draggingPiece = null;
    private Vector3 offset;
    private Dictionary<GameObject, Vector3> solutionCoordinates = new Dictionary<GameObject, Vector3>();
    private bool done;


    void Start()
    {
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

        // Check if all pieces have the same offset
        foreach (GameObject piece in puzzlePieces)
        {
            Vector3 currentOffset = piece.transform.position - solutionCoordinates[piece];
            if ((currentOffset - firstPieceOffset).magnitude > 1f) //tweak this value to adjust the tolerance
            {
                return false; // Puzzle is not solved
            }
        }

        return true; // Puzzle is solved
    }

    // Function to show the completed puzzle
    void ShowCompletedPuzzle()
    {
        // Deactivate all puzzle pieces
        Vector3 newPos = new Vector3(0, 0, 0);
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            puzzlePieces[i].SetActive(false);
            if (i == 6)
            {
                newPos = puzzlePieces[i].transform.position;
            }
        }

        foreach (GameObject piece in puzzlePieces)
        {
            piece.SetActive(false);
        }
 
        // Create a new GameObject for the completed puzzle
        GameObject completedPuzzle = new GameObject("CompletedPuzzle");
        completedPuzzle.transform.position = newPos;

        // Add a SpriteRenderer and assign the solvedSprite
        SpriteRenderer renderer = completedPuzzle.AddComponent<SpriteRenderer>();
        renderer.sprite = solvedSprite;
        renderer.sortingOrder = 10; // Ensure it renders on top
    }

    void CreatePuzzle()
    {

        foreach (Sprite sprite in glassSprites)
        {
            GameObject piece = Instantiate(puzzlePiecePrefab, puzzleArea); 
            piece.name = "Piece " + sprite.name;
            SpriteRenderer renderer = piece.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = sprite;
            }
            puzzlePieces.Add(piece);
        }
    }

    void Scatter()
    {
        Debug.Log("Scattering pieces...");
        float orthoHeight = Camera.main.orthographicSize;
        float orthoWidth = Camera.main.aspect * orthoHeight;


        foreach (GameObject piece in puzzlePieces)
        {
            float pieceWidth = piece.GetComponent<SpriteRenderer>().bounds.size.x;
            float pieceHeight = piece.GetComponent<SpriteRenderer>().bounds.size.y;

            float x = Random.Range(-orthoWidth + pieceWidth / 2, orthoWidth - pieceWidth / 2);
            float y = Random.Range(-orthoHeight + pieceHeight / 2, orthoHeight - pieceHeight / 2);

            piece.transform.position = new Vector3(x, y, 0);

            // float x = Random.Range(-puzzleArea.localScale.x / 2, puzzleArea.localScale.x / 2);
            // float y = Random.Range(-puzzleArea.localScale.y / 2, puzzleArea.localScale.y / 2);
            // piece.transform.localPosition = new Vector3(x, y, 0);
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
            // You can add additional logic here, like showing a message or resetting the puzzle
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

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
        solutionCoordinates[puzzlePieces[0]] = new Vector3(-2.301475f, 1.27161f, 0);
        solutionCoordinates[puzzlePieces[1]] = new Vector3(-1.81313f, 0.08828181f, 0);
        solutionCoordinates[puzzlePieces[2]] = new Vector3(-3.066931f, -0.9741172f, 0); 
        solutionCoordinates[puzzlePieces[3]] = new Vector3(-2.835728f, 0.9076666f, 0);
        solutionCoordinates[puzzlePieces[4]] = new Vector3(-2.280923f, 0.7331243f, 0);
        solutionCoordinates[puzzlePieces[5]] = new Vector3(-2.804205f, -0.0861606f, 0);
        solutionCoordinates[puzzlePieces[6]] = new Vector3(-2.410229f, -0.5065725f, 0);
        solutionCoordinates[puzzlePieces[7]] = new Vector3(-3.054111f, -2.672021f, 0);
        solutionCoordinates[puzzlePieces[8]] = new Vector3(-2.268002f, -1.493492f, 0);
        solutionCoordinates[puzzlePieces[9]] = new Vector3(-1.528731f, -1.013308f, 0);
        solutionCoordinates[puzzlePieces[10]] = new Vector3(-1.827033f, -1.868998f, 0);
        solutionCoordinates[puzzlePieces[11]] = new Vector3(-2.229936f, -2.601764f, 0);
        solutionCoordinates[puzzlePieces[12]] = new Vector3(-2.842438f, -1.884654f, 0);
        solutionCoordinates[puzzlePieces[13]] = new Vector3(-1.830737f, -2.561386f, 0);
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

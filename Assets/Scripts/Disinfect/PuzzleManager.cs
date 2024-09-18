using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }
    public Block[] blocks;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        // Initialize or shuffle blocks if needed
    }

    public void CheckIfPuzzleSolved()
    {
        bool allCorrect = true;
        foreach (var block in blocks)
        {
            if (!block.IsCorrectRotation())
            {
                Debug.Log("Puzzle not solved");
                allCorrect = false;
                break; // Puzzle not solved
            }
            //block.SetColor(Color.green);
        }
        if (allCorrect)
        {
            Debug.Log("Puzzle Solved!");
            SetAllBlocksColor(Color.green);
        }
        // You can trigger a win state here
    }

    private void SetAllBlocksColor(Color color)
    {
        foreach (var block in blocks)
        {
            block.SetColor(color);
        }
    }
}

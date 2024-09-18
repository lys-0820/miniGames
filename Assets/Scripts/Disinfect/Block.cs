using UnityEngine;

public class Block : MonoBehaviour
{
    private int rotationState = 0;
    [SerializeField] private bool isCorrectRotation = false;
    [SerializeField] private int correctRotation = 0;

    void Start()
    {
        SetRandomRotation();
    }

    void OnMouseDown()
    {
        //Debug.Log("click");
        RotateBlock();
    }

    void RotateBlock()
    {
        rotationState = (rotationState + 1) % 4;
        transform.rotation = Quaternion.Euler(0, 0, rotationState * 90); // Rotate in 90-degree increments
        Debug.Log($"当前: {rotationState} 正确: {correctRotation}");
        
        PuzzleManager.Instance.CheckIfPuzzleSolved();
    }

    public bool IsCorrectRotation()
    {
        
        return rotationState == correctRotation; // Change this depending on the correct rotation for the puzzle
        
    }

    public void SetRandomRotation()
    {
        rotationState = Random.Range(0, 4);
        transform.rotation = Quaternion.Euler(0, 0, rotationState * 90); // Rotate in 90-degree increments
    }

    public void SetColor(Color color)
    {
        Debug.Log("set color");
        GetComponent<SpriteRenderer>().color = color;
    }
}
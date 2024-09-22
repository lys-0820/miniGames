using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }
    public Block[,] blocks;
    public int rows = 3;
    public int columns = 4;
    private List<Block> selectedBlocks = new List<Block>();

    public Animator solvedAnimationController;

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
        Debug.Log("PuzzleManager Start called");
        InitializeBlocks();
        RandomizeSelectedBlocks(4);
    }

    void InitializeBlocks()
    {
        blocks = new Block[rows, columns];
        Block[] allBlocks = FindObjectsOfType<Block>();
        if (allBlocks.Length == rows * columns)
        {
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    blocks[i, j] = allBlocks[index];
                    index++;
                }
            }
        }
        else
        {
            Debug.LogError("Block 数量与预期不符！");
        }
    }

    void RandomizeSelectedBlocks(int count)
    {
        Debug.Log("RandomizeSelectedBlocks");
        List<Block> randomizableBlocks = new List<Block>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (blocks[i, j].canRandomRotate)
                {
                    randomizableBlocks.Add(blocks[i, j]);
                }
            }
        }

        selectedBlocks.Clear();
        int rotatedCount = 0;
        while (rotatedCount < count && randomizableBlocks.Count > 0)
        {
            int randomIndex = Random.Range(0, randomizableBlocks.Count);
            Block selectedBlock = randomizableBlocks[randomIndex];
            int randomRotations = Random.Range(1, 4); // 随机旋转1-3次
            for (int j = 0; j < randomRotations; j++)
            {
                selectedBlock.RotateBlock();
            }
            selectedBlocks.Add(selectedBlock);
            randomizableBlocks.RemoveAt(randomIndex);
            rotatedCount++;
            Debug.Log($"旋转方块 {rotatedCount}: 旋转 {randomRotations} 次");
        }
    }

    public void CheckIfPuzzleSolved()
    {
        bool allCorrect = true;
        foreach (Block block in selectedBlocks)
        {
            if (!block.IsCorrectRotation())
            {
                Debug.Log("拼图未解决");
                allCorrect = false;
                return;
            }
        }

        if (allCorrect)
        {
            Debug.Log("拼图已解决！");
            //SetAllBlocksColor(Color.green);
            PlaySolvedAnimation();
            // 在这里可以触发胜利状态
        }
    }

    private void  PlaySolvedAnimation()
    {
        if (solvedAnimationController != null)
        {
            solvedAnimationController.SetTrigger("FixTube");
        }
    }
    private void SetAllBlocksColor(Color color)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                blocks[i, j].SetColor(color);
            }
        }
    }
}
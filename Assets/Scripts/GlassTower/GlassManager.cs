using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    public static GlassManager Instance { get; private set; }
    [SerializeField] private List<GameObject> glassList = new List<GameObject>();
    private List<AutoMove> autoMoves = new List<AutoMove>();
    private int currentIndex = 0;
    private bool isWin = false;
    private bool isGameOver = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterAutoMove(AutoMove autoMove)
    {
        autoMoves.Add(autoMove);
    }

    public AutoMove GetStackTarget(AutoMove current)
    {
        int index = autoMoves.IndexOf(current);
        Debug.Log(index);
        if (index > 0)
        {
            return autoMoves[index - 1];
        }
        return null;
    }

    public void NotifyStacked(AutoMove autoMove)
    {
        int index = autoMoves.IndexOf(autoMove);
        if (index == currentIndex)
        {
            currentIndex++;
            //TODO: change the win condition
            if (currentIndex >= 4)
            {
                Debug.Log("游戏胜利！所有物体都已成功堆叠。");
                isWin = true;
                isGameOver = true;
                GameTimer.Instance.MarkGameAsSuccess();
            }
        }

    }
    public bool GetWinCondition()
    {
        return isWin;
    }
    public void SetWinCondition(bool win)
    {
        isWin = win;
    }
    public bool GetGameOverCondition()
    {
        return isGameOver;
    }
    public void SetGameOverCondition(bool gameOver)
    {
        isGameOver = gameOver;
    }
    public void ClearObject(){
        foreach(GameObject glass in glassList){
            Destroy(glass,2f);
        }

    }
    public void ResetGame(){

    }
}
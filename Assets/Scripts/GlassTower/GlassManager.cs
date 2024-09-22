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
    private void Start(){
        InitializeGlasses();
    }
    private void InitializeGlasses()
    {
        GameObject glass = glassList[2].gameObject;
        glass.GetComponent<Collider2D>().enabled = true;
        for (int i = 3; i < glassList.Count; i++) {
            glassList[i].gameObject.GetComponent<Collider2D>().enabled = false;
        }

    }
    public void RegisterAutoMove(AutoMove autoMove)
    {
        autoMoves.Add(autoMove);
    }

    public AutoMove GetStackTarget(AutoMove current)
    {
        int index = autoMoves.IndexOf(current);
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
            if (currentIndex >= 3)
            {
                isWin = true;
                isGameOver = true;
                GameTimer.Instance.MarkGameAsSuccess();
            }
        }

    }
    public void ActiveNextGlass()
    {
        if (currentIndex < glassList.Count-3)
        {
            GameObject nextGlass = glassList[currentIndex + 3].gameObject;
            if (nextGlass != null)
            {
                nextGlass.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
    public void SetWinCondition(bool win)
    {
        isWin = win;
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

}
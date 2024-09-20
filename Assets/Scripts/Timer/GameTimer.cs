using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; } // 单例

    public float gameDuration = 10f; // length of each game
    private float timeRemaining;
    private bool gameEnded = false;
    private bool gameSuccess = false;

    [SerializeField] private GameObject successWindow; // success window
    [SerializeField] private GameObject failureWindow; // failure window
    [SerializeField] private string nextSceneName; // next scene name

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

    // void Start()
    // {
    //     ResetTimer();
    //     successWindow.SetActive(false);
    //     failureWindow.SetActive(false);
    // }
    void OnEnable()
    {
        ResetTimer();
        successWindow.SetActive(false);
        failureWindow.SetActive(false);
    }
    void Update()
    {
        if (!gameEnded)
        {
            timeRemaining -= Time.deltaTime;
            Debug.Log(timeRemaining);
            if (timeRemaining <= 0)
            {

                EndGame();
            }
        }
    }

    // reset timer
    public void ResetTimer()
    {

        timeRemaining = gameDuration;
        gameEnded = false;
        gameSuccess = false;
    }

    // mark game as success
    public void MarkGameAsSuccess()

    {
        if (!gameEnded)
        {
            gameSuccess = true;
            successWindow.SetActive(true); // 显示成功窗口
       }
    }

    // mark game as failure
    public void MarkGameAsFailure()
    {

        if (!gameEnded)
        {
            gameSuccess = false;
            failureWindow.SetActive(true);
        }
    }


    // when time is up, judge the game result
    void EndGame()
    {

        gameEnded = true;

        if (gameSuccess)
        {
            // game success, load next scene
            Invoke("LoadNextScene", 2f); // delay 2 seconds to load next scene

        }
        else
        {
            // game failure, show failure window and load this scene
            failureWindow.SetActive(true);
            Invoke("LoadThisScene", 2f);

        }
    }
    // restart this game
    void LoadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //ResetTimer();
    }
    // load next scene
    void LoadNextScene()
    {
        //TODO: change scene
        //int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneName);
        
    }


    // wait for remaining time and load next scene
    void WaitAndLoadNextScene()
    {

        if (!gameEnded)
        {
            Invoke("LoadNextScene", timeRemaining);
        }
    }
}
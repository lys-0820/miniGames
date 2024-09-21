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

    //skip
    private static int retryTimes = 0;
    [SerializeField] private GameObject skipButton;

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

    void OnEnable()
    {
        ResetTimer();
        transform.gameObject.SetActive(true);
        skipButton.SetActive(false);
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
            //TODO: change success window
            //transform.gameObject.SetActive(true);
            successWindow.SetActive(true); // show success window
       }
    }

    // mark game as failure
    public void MarkGameAsFailure()
    {

        if (!gameEnded)
        {
            gameSuccess = false;
            //transform.gameObject.SetActive(true);
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
            //TODO: change success window
            Invoke("LoadNextScene", 2f); // delay 2 seconds to load next scene

        }
        else
        {
            // game failure, show failure window and load this scene
            retryTimes++;
            Debug.Log("retryTimes: " + retryTimes);
            if(retryTimes >= 3){
                skipButton.SetActive(true);
        }
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
        SceneManager.LoadScene(nextSceneName);
        
    }

    // skip
    public void SkipGame(){
        retryTimes = 0;
        skipButton.SetActive(false);
        LoadNextScene();
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

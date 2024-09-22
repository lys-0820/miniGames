using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; } // 单例

    public float gameDuration = 10f; // length of each game
    private float timeRemaining;
    private bool gameEnded = false;
    private bool gameSuccess = false;
    private string nextSceneName;
    //skip
    private static int retryTimes = 0;
    [SerializeField] private GameObject skipButton;
    private bool isSkipping = false;
    //time text & image
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image timeImage;
    //description text
    [SerializeField] private GameDescriptions gameDescriptions;
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private string[] sceneNames;
    
    //success and failure window
    [SerializeField] private GameObject successWindow; // success window
    [SerializeField] private GameObject failureWindow; // failure window
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource successMusic;
    [SerializeField] private AudioSource failureMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    void OnEnable()
    {
        isSkipping = false;
        ResetTimer();
        timeImage.gameObject.SetActive(true);
        transform.gameObject.SetActive(true);
        descriptionWindow.SetActive(false);
        skipButton.SetActive(false);
        successWindow.SetActive(false);
        failureWindow.SetActive(false);
    }
    void Update()
    {
        if (!gameEnded)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeText();
            if (timeRemaining <= 0)
            {

                EndGame();
            }
        }
    }

    // update time text & image
    void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = Mathf.Ceil(timeRemaining).ToString();
        }
        if (timeImage != null)
        {
            timeImage.fillAmount = timeRemaining / gameDuration;
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
       }


    }

    // mark game as failure
    public void MarkGameAsFailure()
    {

        if (!gameEnded)
        {
            gameSuccess = false;
        }
    }


    // when time is up, judge the game result
    void EndGame()
    {
        if(isSkipping){
            return;
        }
        gameEnded = true;
        if(bgMusic != null){
            bgMusic.Stop();
        }
        if (gameSuccess)
        {

            // game success, load next scene
            if(successMusic != null){
                successMusic.Play();
            }
            timeImage.gameObject.SetActive(false);
            successWindow.SetActive(true); // show success window
            //TODO: change success window
            Invoke("LoadNextScene", 2f); // delay 2 seconds to load next scene

        }
        else
        {
            // game failure, show failure window and load this scene
            retryTimes++;
            Debug.Log("retryTimes: " + retryTimes);

            //failureWindow.SetActive(true);
            Invoke("ShowFailureWindow", 2f);
            

        }
    }
    void ShowFailureWindow(){
        timeImage.gameObject.SetActive(false);
        failureWindow.SetActive(true);
        if(failureMusic != null){
            failureMusic.Play();
        }
        if(retryTimes >= 3){
            skipButton.SetActive(true);

        }
        Invoke("LoadThisScene", 2f);
    }
    void CloseFailureWindow(){
        failureWindow.SetActive(false);
        skipButton.SetActive(false);
    }
    void CloseSuccessWindow(){
        successWindow.SetActive(false);
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
        //SceneManager.LoadScene(nextSceneName);
        nextSceneName = GetRandomGame();
        string des = GetExplanationForScene(nextSceneName);
        if (!string.IsNullOrEmpty(des))
        {   
            CloseFailureWindow();
            CloseSuccessWindow();
            descriptionWindow.SetActive(true);
            descriptionText.text = des;
            descriptionText.gameObject.SetActive(true);
            StartCoroutine(ActuallyLoadNextScene(4f));
        }
        else
        {
            ActuallyLoadNextScene();
        }
    }

    private string GetRandomGame()
    {
        var index  = Random.Range(0, sceneNames.Length);
        return sceneNames[index];
    }
    
    string GetExplanationForScene(string sceneName)
    {
        if (gameDescriptions != null)
        {
            foreach (var description in gameDescriptions.gameDescriptions)
            {
                if (description.sceneName == sceneName)
                {
                    return description.descriptionText;
                }
            }
        }
        return null;
    }

    IEnumerator ActuallyLoadNextScene(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        descriptionText.gameObject.SetActive(false);
        descriptionWindow.SetActive(false);
        Destroy(this.gameObject);
    }

    // skip
    public void SkipGame(){
        isSkipping = true;
        retryTimes = 0;
        skipButton.SetActive(false);
        CancelInvoke();
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
    public float GetRemainingTime(){
        return timeRemaining;
    }
    public bool GetGameSuccess(){
        return gameSuccess;
    }
}

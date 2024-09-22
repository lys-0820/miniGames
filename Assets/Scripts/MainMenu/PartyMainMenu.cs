using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameDescriptions;
using TMPro;
public class PartyMainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    private string firstSceneName;
    //description text
    [SerializeField] private string[] sceneNames;

    [SerializeField] private GameDescriptions gameDescriptions;
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] private TMP_Text descriptionText;
    void Start()
    {
        firstSceneName = GetRandomGame();
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        descriptionWindow.gameObject.SetActive(false);
    }
    
    private string GetRandomGame()
    {
        var index  = Random.Range(0, sceneNames.Length);
        return sceneNames[index];
    }


    private void StartGame()
    {
        LoadNextScene();
        //SceneManager.LoadScene(firstSceneName);
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    void LoadNextScene()
    {
        //SceneManager.LoadScene(nextSceneName);
        string des = GetExplanationForScene(firstSceneName);
        if (!string.IsNullOrEmpty(des))
        {
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
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(firstSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        descriptionText.gameObject.SetActive(false);
        descriptionWindow.SetActive(false);
        Destroy(this.gameObject);
    }
}

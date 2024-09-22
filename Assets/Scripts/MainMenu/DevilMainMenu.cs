using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class DevilMainMenu : MonoBehaviour
{
    private string firstSceneName;
    [SerializeField] private GameDescriptions gameDescriptions;
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private string[] sceneNames;
    void Start()
    {
        firstSceneName = GetRandomGame();
        descriptionWindow.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        LoadNextScene();
    }
    public void ExitGame()
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

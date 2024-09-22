using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class DevilMainMenu : MonoBehaviour
{
    private string firstSceneName;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameDescriptions gameDescriptions;
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private string[] sceneNames;
    void Start()
    {
        
        descriptionWindow.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        _animator.SetBool("start", true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadNextScene()
    {
        //SceneManager.LoadScene(nextSceneName);
        firstSceneName = GetRandomGame();
        string des = GetExplanationForScene(firstSceneName);
        Debug.Log(firstSceneName);
        Debug.Log(des);

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
        Debug.Log(firstSceneName);
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

using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class DevilMainMenu : MonoBehaviour
{
    [SerializeField] private string firstSceneName;
    [SerializeField] private GameDescriptions gameDescriptions;
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] private TMP_Text descriptionText;
    void Start()
    {
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
            Invoke("ActuallyLoadNextScene", 4f);
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

    void ActuallyLoadNextScene()
    {
        descriptionText.gameObject.SetActive(false);
        descriptionWindow.SetActive(false);
        SceneManager.LoadScene(firstSceneName);
    }
}

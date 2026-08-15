using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;

    private void Start()
    {
        infoPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Level");
    }

    public void OpenInfo()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
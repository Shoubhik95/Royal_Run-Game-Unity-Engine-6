using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameoverText;
    [SerializeField] GameObject restartButton;

    [Header("Game Over Sound")]
    [SerializeField] private AudioSource gameOverAudioSource;

    [SerializeField] float startTime = 5f;

    float timeleft;
    bool gameOver = false;

    public bool GameOver => gameOver;

    void Start()
    {
        timeleft = startTime;

        gameoverText.SetActive(false);
        restartButton.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        bool flowControl = DecreaseTime();

        if (!flowControl)
        {
            return;
        }
    }

    public void IncreaseTime(float amount)
    {
        timeleft += amount;
    }

    public bool ReturnGameOver()
    {
        return gameOver;
    }

    private bool DecreaseTime()
    {
        if (gameOver)
            return false;

        timeleft -= Time.deltaTime;
        timeText.text = timeleft.ToString("F1");

        if (timeleft <= 0f)
        {
            PlayerGameOver();
        }

        return true;
    }

    private void PlayerGameOver()
    {
        gameOver = true;

        playerController.enabled = false;

        gameoverText.SetActive(true);
        restartButton.SetActive(true);

        // Play Game Over sound
        if (gameOverAudioSource != null)
        {
            gameOverAudioSource.Play();
        }

        Time.timeScale = 0.1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main_Level");
    }
}
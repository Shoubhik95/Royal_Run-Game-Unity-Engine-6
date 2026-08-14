using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameoverText;
    [SerializeField] float startTime = 5f;

    float timeleft;
    bool gameOver = false;

    //GETSET METHOD 
    //public bool GameOver
    //{
    //    get { return gameOver; }
    //    //set { gameOver = value; }
    //}

    //public bool GameOver { get; private set; }

    public bool GameOver => gameOver;




    void Start()
    {
        timeleft = startTime;
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

    void PlayerGameOver()
    {
        gameOver = true;
        playerController.enabled = false;
        gameoverText.SetActive(true);
        Time.timeScale = 0.1f;
    }
}

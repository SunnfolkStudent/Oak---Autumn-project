using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private float timeCounter;
    [SerializeField] private float countdownTimer = 120f;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
    [SerializeField] private bool isCountdown;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private HighScoreController highScoreController;

    private int timerThreashhold = 1;

    private void Start()
    {
        highScoreController = GameObject.Find("HighScoreManager").GetComponent<HighScoreController>();
    }
    
    private void Update()
    {
        if (timeCounter >= timerThreashhold)
        {
            print(PlayerPrefs.GetInt("CurrentScore"));
            highScoreController.UpdateScore();
            timerThreashhold++;
        }
        if (isCountdown && countdownTimer > 0)
        {
            timeCounter -= Time.deltaTime;
            minutes = Mathf.FloorToInt(countdownTimer / 60f);
            seconds = Mathf.FloorToInt(countdownTimer - minutes * 60);
        }
        else if (!isCountdown)
        {
            timeCounter += Time.deltaTime;
            minutes = Mathf.FloorToInt(timeCounter / 60f);
            seconds = Mathf.FloorToInt(timeCounter - minutes * 60);
        }
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

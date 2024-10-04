using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreController : MonoBehaviour
{
    private int highScore = 0;

    private int beginningHighScore;

    private int currentHighScore;
    
    [SerializeField] private TextMeshProUGUI HighScoreTexst;
    [SerializeField] private TextMeshProUGUI MenuScoreTexst;

    public void Start()
    {
        beginningHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (SceneManager.GetActiveScene().name != "VictoryScreen")
        {
            PlayerPrefs.SetInt("CurrentScore", 0);
        }
        
    }
    
    public void UpdateScore()
    {
        highScore++;
        PlayerPrefs.SetInt("CurrentScore", 100 - highScore);
    }

    public void MenuScore()
    {
        if (PlayerPrefs.GetInt("CurrentScore") > beginningHighScore)
        {
            highScore = (100 - PlayerPrefs.GetInt("CurrentScore"));
            print("New High Score");
            PlayerPrefs.SetInt("HighScore", 100 - highScore);
            HighScoreTexst.text = "New High Score!!";
            MenuScoreTexst.text = "Your Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
            return;
        }

        if (PlayerPrefs.GetInt("CurrentScore") <= PlayerPrefs.GetInt("HighScore"))
        {
            print("Not High Score");
            HighScoreTexst.text = "Your Score:";
            //MenuScoreTexst.text = PlayerPrefs.GetInt("CurrentScore", 0).ToString();
            MenuScoreTexst.text = PlayerPrefs.GetInt("CurrentScore", 0).ToString();
            //MenuScoreTexst.text = "New High Score!";
        }
    }
}

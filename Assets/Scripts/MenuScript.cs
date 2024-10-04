using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameObject o;

    private HighScoreController highScript;
    
    void Start()
    {
        o = GameObject.Find("HighScoreManager");
        highScript = o.GetComponent<HighScoreController>();
        
        if (SceneManager.GetActiveScene().name == "VictoryScreen")
        {
            highScript.MenuScore();
        }
    }
    
    public void GameScene()
    {
        SceneManager.LoadScene("FUCKOFF ROBIN");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void VictoryScreen()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("Workspace_Maya");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void VictoryScreen()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit()
#endif
    }
}

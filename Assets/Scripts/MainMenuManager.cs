using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        // Load the level stored in PlayerPrefs
        int lastLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        SceneManager.LoadScene("Level" + lastLevel);  // Assuming your levels are named Level1, Level2, etc.
    }
}

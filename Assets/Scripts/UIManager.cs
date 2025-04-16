using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu()
    {
        FindFirstObjectByType<UpgradeManager>().SaveProgress();
        SceneManager.LoadScene("Main Menu");
    }
}
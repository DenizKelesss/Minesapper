using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void RestartGame()
    {
        // Tutorial marked complete so it won’t replay or restore deleted blocks
        TutorialManager.tutorialAlreadyCompleted = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu()
    {
        FindFirstObjectByType<UpgradeManager>()?.SaveProgress();
        SceneManager.LoadScene("Main Menu");
    }
}

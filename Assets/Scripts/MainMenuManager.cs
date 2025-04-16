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
        string path = Application.persistentDataPath + "/savefile.json";

        int lastLevel = 1; // Default level
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            lastLevel = data.currentLevel;
        }

        SceneManager.LoadScene("Level" + lastLevel);
    }
}

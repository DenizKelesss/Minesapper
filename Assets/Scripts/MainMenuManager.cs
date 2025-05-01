using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button continueButton;

    private void Start()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (System.IO.File.Exists(path))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }



    public void StartGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        int lastLevel = 1; //default
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            lastLevel = data.currentLevel;
        }
        else
        {
            Debug.Log("No saved game found."); // could be nice to add a little ui for this, later.
        }

        SceneManager.LoadScene("Level" + lastLevel);
    }
}

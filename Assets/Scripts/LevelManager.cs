using UnityEngine;
using UnityEngine.SceneManagement;

// NO LONGER USED - KEPT JUST IN CASE. IN CASE WHAT? DUNNO.

public class LevelManager : MonoBehaviour
{
    /*
    public GameManager gameManager;

    public void Start()
    {
        // Check if the active scene is "Level1"
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            LoadLevel(1);
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            LoadLevel(2);
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            LoadLevel(2);
        }
    }

    public void LoadLevel(int level)
    {
        gameManager.ClearField();

        switch (level)
        {
            case 1:
                gameManager.width = 6;
                gameManager.height = 6;
                gameManager.mineCount = 4;
                SetMineDestructorChance(60f);  // higher chance in early levels?
                break;
            case 2:
                gameManager.width = 8;
                gameManager.height = 8;
                gameManager.mineCount = 10;
                SetMineDestructorChance(60f);  // higher chance in early levels?
                break;
            case 3:
                gameManager.width = 12;
                gameManager.height = 12;
                gameManager.mineCount = 20;
                SetMineDestructorChance(40f);
                break;
            case 4:
                gameManager.width = 16;
                gameManager.height = 16;
                gameManager.mineCount = 40;
                SetMineDestructorChance(25f);
                break;
            default:
                gameManager.width = 8;
                gameManager.height = 8;
                gameManager.mineCount = 10;
                SetMineDestructorChance(50f);
                break;
        }

        gameManager.GenerateField();
    }

    void SetMineDestructorChance(float chance)
    {
        MineDestructor destructor = FindFirstObjectByType<MineDestructor>();
        if (destructor != null)
        {
            destructor.destroyChance = chance;
        }
    }
    */
}

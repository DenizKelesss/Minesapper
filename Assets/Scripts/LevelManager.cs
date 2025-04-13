using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;

    public void Start()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 1:
                gameManager.width = 8;
                gameManager.height = 8;
                gameManager.mineCount = 10;
                break;
            case 2:
                gameManager.width = 12;
                gameManager.height = 12;
                gameManager.mineCount = 20;
                break;
            case 3:
                gameManager.width = 16;
                gameManager.height = 16;
                gameManager.mineCount = 40;
                break;
            default:
                gameManager.width = 8;
                gameManager.height = 8;
                gameManager.mineCount = 10;
                break;
        }

        gameManager.ClearField();
        gameManager.GenerateField();
    }
}

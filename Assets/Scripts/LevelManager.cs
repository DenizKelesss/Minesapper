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
        gameManager.ClearField();

        switch (level)
        {
            case 1:
                gameManager.width = 8;
                gameManager.height = 8;
                gameManager.mineCount = 2;
                SetMineDestructorChance(60f);  // higher chance in early levels?
                break;
            case 2:
                gameManager.width = 12;
                gameManager.height = 12;
                gameManager.mineCount = 20;
                SetMineDestructorChance(40f);
                break;
            case 3:
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
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextSceneIndex = currentScene.buildIndex + 1;

        if (other.CompareTag("Player") && nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            FindFirstObjectByType<UpgradeManager>().LevelUp(); // needed to save this type of level too, it should be saved. I think.
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
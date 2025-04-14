using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Update method to constantly check if all "Mine" objects are destroyed
    void Update()
    {
        // Find all objects with the "Mine" tag
        GameObject[] mineObjects = GameObject.FindGameObjectsWithTag("Mine");

        // If there are no objects with the "Mine" tag left, trigger scene transition
        if (mineObjects.Length == 0)
        {
            // Get the current active scene
            Scene currentScene = SceneManager.GetActiveScene();

            // Load the next scene by index
            int nextSceneIndex = currentScene.buildIndex + 1;

            // Check if the next scene exists in the build settings
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes to load!");
            }
        }
    }
}

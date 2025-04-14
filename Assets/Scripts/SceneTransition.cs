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
            // Load the next scene (or specify a scene name/ID)
            SceneManager.LoadScene("Level2"); // Replace "NextSceneName" with the actual scene name
        }
    }
}

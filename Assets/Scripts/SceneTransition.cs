using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public TextMeshProUGUI statusText;         // Assign in Inspector.
    public TextMeshProUGUI countdownText;      // Assign a second Text UI for countdown.

    private bool isTransitioning = false;

    void Update()
    {
        if (isTransitioning) return;

        GameObject[] mineObjects = GameObject.FindGameObjectsWithTag("Mine");

        if (mineObjects.Length == 0)
        {
            FindFirstObjectByType<UpgradeManager>().LevelUp(); // Invoke level up method here once a level is cleared (a cleared level invokes scene transition)
            isTransitioning = true;
            if (statusText != null)
            {
                statusText.text = "Level Cleared!";
            }
            StartCoroutine(LoadNextSceneWithCountdown(5f));  // 3 seconds countdown.
        }
    }

    private IEnumerator LoadNextSceneWithCountdown(float delay)
    {
        float remainingTime = delay;

        while (remainingTime > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = "Next Level In: " + Mathf.CeilToInt(remainingTime).ToString();
            }

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        if (countdownText != null)
        {
            countdownText.text = "";  // Clear after countdown.
        }

        Scene currentScene = SceneManager.GetActiveScene();
        int nextSceneIndex = currentScene.buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes to load!");
            if (statusText != null)
            {
                statusText.text = "No more scenes to load!";
            }
        }
    }
}

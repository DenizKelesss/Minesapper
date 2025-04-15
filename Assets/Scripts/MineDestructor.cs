using TMPro;
using Unity.UI;
using UnityEngine;

public class MineDestructor : MonoBehaviour
{
    [Range(0, 100)]
    public float destroyChance = 50f;

    public TextMeshProUGUI statusText;
    public TextMeshPro detectorStatusText;


    private Collider currentMine;
    private MineMinigame currentMinigame;
    private bool isMinigameActive = false;

    private void Start()
    {
        currentMinigame = FindFirstObjectByType<MineMinigame>();
        if (currentMinigame == null)
        {
            Debug.LogError("MineMinigame is missing from the scene! Please add it.");
        }
    }

    private void Update()
    {
        if (isMinigameActive) return;  // Block input while minigame is active.

        if (currentMine != null && Input.GetKeyDown(KeyCode.E))
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= destroyChance)
            {
                UpdateStatus("Mine destroyed successfully!");
                UpdateDetectorStatus("Mine destroyed successfully!");
                Destroy(currentMine.gameObject);
                currentMine = null;
            }
            else
            {
                UpdateStatus("Failed to destroy the mine! Triggering minigame...");
                UpdateDetectorStatus("Failed to destroy the mine! Triggering minigame...");

                if (currentMinigame != null)
                {
                    isMinigameActive = true;
                    Collider mineToDestroy = currentMine;
                    currentMine = null;

                    currentMinigame.StartMinigame(
                        () => OnMinigameSuccess(mineToDestroy),
                        () => OnMinigameFailure(mineToDestroy),
                        15f // Example: 15 seconds, or fetch from GameManager for future upgrades
                    );
                }
                else
                {
                    Debug.LogWarning("No MineMinigame object found in the scene!");
                }
            }
        }
    }

    private void OnMinigameFailure(Collider mine)
    {
        if (mine != null)
        {
            UpdateStatus("Mine exploded during the minigame!");
            UpdateDetectorStatus("Mine exploded during the minigame!");
            Destroy(mine.gameObject);
            if (currentMine == mine)
            {
                currentMine = null;
            }
        }
        isMinigameActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            currentMine = other;
            UpdateStatus("Mine in range. Press [E] to attempt destruction.");
            UpdateDetectorStatus("Mine in range. Press [E] to attempt destruction");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentMine)
        {
            UpdateStatus("Left mine range.");
            UpdateDetectorStatus("Left mine range");

            currentMine = null;
        }
    }

    private void OnMinigameSuccess(Collider mine)
    {
        if (mine != null)
        {
            UpdateStatus("Mine defused!");
            UpdateDetectorStatus("Mine Defused");
            Destroy(mine.gameObject);
            if (currentMine == mine)
            {
                currentMine = null;
            }

        }
        isMinigameActive = false;  // Unlock input after minigame ends.
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void UpdateDetectorStatus(string message)
    {
        if (detectorStatusText != null)
        {
            detectorStatusText.text = message;
        }
    }
}

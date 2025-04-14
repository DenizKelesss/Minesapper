using UnityEngine;

public class MineDestructor : MonoBehaviour
{
    [Range(0, 100)]
    public float destroyChance = 50f;

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
                Debug.Log("Mine destroyed successfully!");
                Destroy(currentMine.gameObject);
                currentMine = null;
            }
            else
            {
                Debug.Log("Failed to destroy the mine! Triggering minigame...");
                if (currentMinigame != null)
                {
                    isMinigameActive = true;
                    Collider mineToDestroy = currentMine;
                    currentMine = null;

                    currentMinigame.StartMinigame(() => OnMinigameSuccess(mineToDestroy));
                }
                else
                {
                    Debug.LogWarning("No MineMinigame object found in the scene!");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            currentMine = other;
            Debug.Log("Mine in range. Press [E] to attempt destruction.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentMine)
        {
            Debug.Log("Left mine range.");
            currentMine = null;
        }
    }

    private void OnMinigameSuccess(Collider mine)
    {
        if (mine != null)
        {
            Debug.Log("Minigame success — mine defused!");
            Destroy(mine.gameObject);
            if (currentMine == mine)
            {
                currentMine = null;
            }
        }
        isMinigameActive = false;  // Unlock input after minigame ends.
    }
}

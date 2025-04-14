using UnityEngine;

public class MineDestructor : MonoBehaviour
{
    [Range(0, 100)]
    public float destroyChance = 50f;

    private Collider currentMine;

    public delegate void MinigameStart();
    public static event MinigameStart OnMinigameStart;

    private void Update()
    {
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
                OnMinigameStart?.Invoke();
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
}

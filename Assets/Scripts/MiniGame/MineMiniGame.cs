using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class MineMinigame : MonoBehaviour
{
    public GameObject minigameUI;
    public RectTransform draggableObject;
    public RectTransform targetArea;
    public RectTransform mazeArea;
    public GameObject wallPrefab;
    public int numberOfWalls = 5;
    public float minigameTime = 10f;  // Default time
    public TextMeshProUGUI timerText;

    private System.Action onSuccess;
    private System.Action onFailure;
    private bool isDragging = false;
    private float timeRemaining;
    private List<GameObject> activeWalls = new List<GameObject>();

    private FirstPersonPlayer player;


    public void StartMinigame(System.Action successCallback, System.Action failureCallback, float customTime = -1f)
    {
        player = FindFirstObjectByType<FirstPersonPlayer>();
        if (player != null)
        {
            player.canMove = false;
        }

        onSuccess = successCallback;
        onFailure = failureCallback;
        minigameUI.SetActive(true);
        draggableObject.anchoredPosition = Vector2.zero;

        timeRemaining = (customTime > 0) ? customTime : minigameTime;

        GenerateMazeWalls();
    }

    private void Update()
    {
        if (!minigameUI.activeSelf) return;

        // Timer logic
        timeRemaining -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
        }
        if (timeRemaining <= 0)
        {
            MinigameFailed();
            return;
        }

        // Dragging logic
        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(draggableObject, Input.mousePosition))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, Input.mousePosition))
            {
                MinigameSuccess();
            }
        }

        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minigameUI.GetComponent<RectTransform>(),
                Input.mousePosition,
                null,
                out Vector2 localPoint
            );
            draggableObject.anchoredPosition = localPoint;

            if (CheckWallCollision())
            {
                MinigameFailed();
            }
        }
    }

    private void GenerateMazeWalls()
    {
        ClearWalls();

        for (int i = 0; i < numberOfWalls; i++)
        {
            GameObject wall = Instantiate(wallPrefab, mazeArea);
            RectTransform wallRect = wall.GetComponent<RectTransform>();

            wallRect.anchoredPosition = new Vector2(
                Random.Range(-mazeArea.rect.width / 2 + 50, mazeArea.rect.width / 2 - 50),
                Random.Range(-mazeArea.rect.height / 2 + 50, mazeArea.rect.height / 2 - 50)
            );

            wallRect.sizeDelta = new Vector2(
                Random.Range(50f, 150f),
                Random.Range(10f, 50f)
            );

            MiniGameWallMotion mover = wall.GetComponent<MiniGameWallMotion>();
            if (mover != null)
            {
                mover.moveDirection = Random.insideUnitCircle.normalized;
                mover.moveDistance = Random.Range(20f, 100f);
                mover.moveSpeed = Random.Range(20f, 60f);
            }

            activeWalls.Add(wall);
        }
    }

    private bool CheckWallCollision()
    {
        foreach (GameObject wall in activeWalls)
        {
            RectTransform wallRect = wall.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(wallRect, Input.mousePosition))
            {
                return true;
            }
        }
        return false;
    }

    private void ClearWalls()
    {
        foreach (var wall in activeWalls)
        {
            Destroy(wall);
        }
        activeWalls.Clear();
    }

    private void MinigameSuccess()
    {
        Debug.Log("Minigame Success!");
        minigameUI.SetActive(false);
        ClearWalls();
        onSuccess?.Invoke();

        if (player != null)
        {
            player.canMove = true;
        }

        var upgradeManager = FindFirstObjectByType<UpgradeManager>();
        if (upgradeManager != null)
        {
            int xpGained = Random.Range(5, 11);
            upgradeManager.GainXP(xpGained);
        }
    }

    private void MinigameFailed()
    {
        Debug.Log("Minigame Failed! Mine will still be destroyed.");
        minigameUI.SetActive(false);
        ClearWalls();
        onFailure?.Invoke();

        if (player != null)
        {
            player.canMove = true;
        }

        var upgradeManager = FindFirstObjectByType<UpgradeManager>();
        int damage = upgradeManager != null ? upgradeManager.GetFailDamage() : 3;

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.DecreaseHealth(damage);  // Decrease by damage amount on each fail.
        }
    }
}

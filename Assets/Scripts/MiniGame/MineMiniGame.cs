using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MineMiniGame : MonoBehaviour, IMinigame
{
    public GameObject minigameUI;
    public RectTransform draggableObject;
    public RectTransform targetArea;
    public RectTransform mazeArea;
    public GameObject wallPrefab;
    public int numberOfWalls = 5;
    public float minigameTime = 10f;
    public TextMeshProUGUI timerText;

    [Header("Wall Size Settings")]
    public float uniformSizeMin = 50f;
    public float uniformSizeMax = 150f;

    private Action onSuccess;
    private Action onFailure;
    private bool isDragging = false;
    private float timeRemaining;
    private List<GameObject> activeWalls = new List<GameObject>();
    private FirstPersonPlayer player;

    public void StartMinigame(Action successCallback, Action failureCallback, float customTime = -1f)
    {
        player = FindFirstObjectByType<FirstPersonPlayer>();
        if (player != null) player.canMove = false;

        onSuccess = successCallback;
        onFailure = failureCallback;
        minigameUI.SetActive(true);
        draggableObject.anchoredPosition = new Vector2 (-854, 0);
        timeRemaining = (customTime > 0) ? customTime : minigameTime;
        GenerateMazeWalls();
    }

    private void Update()
    {
        if (!minigameUI.activeSelf) return;

        // Timer
        timeRemaining -= Time.deltaTime;
        if (timerText != null) timerText.text = "Time: " + Mathf.Ceil(timeRemaining);
        if (timeRemaining <= 0) { MinigameFailed(); return; }

        // Drag start
        if (Input.GetMouseButtonDown(0) &&
            RectTransformUtility.RectangleContainsScreenPoint(draggableObject, Input.mousePosition))
        {
            isDragging = true;
        }

        // Drag end & check for success
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging && RectTransformUtility.RectangleContainsScreenPoint(targetArea, Input.mousePosition))
            {
                MinigameSuccess();
            }
            isDragging = false;
        }

        // Drag movement & collision detection
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minigameUI.GetComponent<RectTransform>(),
                Input.mousePosition, null, out Vector2 localPoint);
            draggableObject.anchoredPosition = localPoint;

            if (CheckWallCollision())
            {
                MinigameFailed();
                isDragging = false;
            }
        }
    }

    private void GenerateMazeWalls()
    {
        ClearWalls();

        float edgeBuffer = 100f; // margin so walls don't get stuck on edges

        for (int i = 0; i < numberOfWalls; i++)
        {
            GameObject wall = Instantiate(wallPrefab, mazeArea);
            RectTransform wr = wall.GetComponent<RectTransform>();

            // Position walls inside panel with buffer margin
            wr.anchoredPosition = new Vector2(
                Random.Range(-mazeArea.rect.width / 2 + edgeBuffer, mazeArea.rect.width / 2 - edgeBuffer),
                Random.Range(-mazeArea.rect.height / 2 + edgeBuffer, mazeArea.rect.height / 2 - edgeBuffer)
            );

            // Uniform square size in range
            float uniformSize = Random.Range(uniformSizeMin, uniformSizeMax);
            wr.sizeDelta = new Vector2(uniformSize, uniformSize);

            // Setup wall movement
            var mover = wall.GetComponent<MiniGameWallMotion>();
            if (mover != null)
            {
                mover.moveDirection = Random.insideUnitCircle.normalized;
                mover.moveDistance = Random.Range(150f, 450f);  // perimeter range
                mover.moveSpeed = Random.Range(75f, 150f);     // movement speed
            }

            activeWalls.Add(wall);
        }
    }

    private bool CheckWallCollision()
    {
        foreach (var wall in activeWalls)
        {
            var wr = wall.GetComponent<RectTransform>();
            // Check if draggableObject overlaps any wall rect (using screen points)
            if (RectOverlaps(draggableObject, wr))
                return true;
        }
        return false;
    }

    // Helper for RectTransform overlap check
    private bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Vector3[] aCorners = new Vector3[4];
        Vector3[] bCorners = new Vector3[4];
        a.GetWorldCorners(aCorners);
        b.GetWorldCorners(bCorners);

        Rect aRect = new Rect(aCorners[0], aCorners[2] - aCorners[0]);
        Rect bRect = new Rect(bCorners[0], bCorners[2] - bCorners[0]);

        return aRect.Overlaps(bRect);
    }

    private void ClearWalls()
    {
        foreach (var w in activeWalls) Destroy(w);
        activeWalls.Clear();
    }

    private void MinigameSuccess()
    {
        EndMinigame();
        onSuccess?.Invoke();
        var um = FindFirstObjectByType<UpgradeManager>();
        if (um != null) um.GainXP(UnityEngine.Random.Range(5, 11));
    }

    private void MinigameFailed()
    {
        EndMinigame();
        onFailure?.Invoke();
        var um = FindFirstObjectByType<UpgradeManager>();
        int dmg = (um != null) ? um.GetFailDamage() : 3;
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null) ph.DecreaseHealth(dmg);
    }

    private void EndMinigame()
    {
        minigameUI.SetActive(false);
        ClearWalls();
        if (player != null) player.canMove = true;
    }
}
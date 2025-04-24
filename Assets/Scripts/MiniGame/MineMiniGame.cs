// Assets/Scripts/MineMinigame.cs
using UnityEngine;
using UnityEngine.UI;
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
        draggableObject.anchoredPosition = Vector2.zero;
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

        // Drag start/stop
        if (Input.GetMouseButtonDown(0) &&
            RectTransformUtility.RectangleContainsScreenPoint(draggableObject, Input.mousePosition))
            isDragging = true;

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, Input.mousePosition))
                MinigameSuccess();
        }

        // Drag movement & collision
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minigameUI.GetComponent<RectTransform>(),
                Input.mousePosition, null, out Vector2 localPoint);
            draggableObject.anchoredPosition = localPoint;

            if (CheckWallCollision()) MinigameFailed();
        }
    }

    private void GenerateMazeWalls()
    {
        ClearWalls();
        for (int i = 0; i < numberOfWalls; i++)
        {
            GameObject wall = Instantiate(wallPrefab, mazeArea);
            RectTransform wr = wall.GetComponent<RectTransform>();
            wr.anchoredPosition = new Vector2(
                Random.Range(-mazeArea.rect.width / 2 + 50, mazeArea.rect.width / 2 - 50),
                Random.Range(-mazeArea.rect.height / 2 + 50, mazeArea.rect.height / 2 - 50)
            );
            wr.sizeDelta = new Vector2(
                Random.Range(50f, 150f),
                Random.Range(10f, 50f)
            );

            var mover = wall.GetComponent<MiniGameWallMotion>();
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
        foreach (var wall in activeWalls)
        {
            var wr = wall.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(wr, Input.mousePosition))
                return true;
        }
        return false;
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

using UnityEngine;
using UnityEngine.UI;

public class MineMinigame : MonoBehaviour
{
    public GameObject minigameUI;
    public RectTransform draggableObject;
    public RectTransform targetArea;

    private System.Action onSuccess;
    private bool isDragging = false;

    public void StartMinigame(System.Action successCallback)
    {
        onSuccess = successCallback;
        minigameUI.SetActive(true);
        draggableObject.anchoredPosition = Vector2.zero; // reset start position
    }

    void Update()
    {
        if (!minigameUI.activeSelf) return;

        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(draggableObject, Input.mousePosition))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, Input.mousePosition))
            {
                Debug.Log("Minigame Success!");
                minigameUI.SetActive(false);
                onSuccess?.Invoke();
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
        }
    }
}

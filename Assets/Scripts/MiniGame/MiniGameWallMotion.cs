using UnityEngine;

public class MiniGameWallMotion : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveDistance;
    public float moveSpeed;

    private Vector2 startPos;
    private bool movingForward = true;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        Vector2 targetPos = movingForward
            ? startPos + moveDirection * moveDistance
            : startPos;

        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPos) < 0.1f)
        {
            movingForward = !movingForward;
        }
    }
}
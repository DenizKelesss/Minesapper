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
        if (movingForward)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(
                rectTransform.anchoredPosition,
                startPos + moveDirection * moveDistance,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(rectTransform.anchoredPosition, startPos + moveDirection * moveDistance) < 0.1f)
                movingForward = false;
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(
                rectTransform.anchoredPosition,
                startPos,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(rectTransform.anchoredPosition, startPos) < 0.1f)
                movingForward = true;
        }
    }
}

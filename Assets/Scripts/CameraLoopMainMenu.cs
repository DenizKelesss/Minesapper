using UnityEngine;

public class LoopingCamera : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = new Vector3(1, 0, 0); // Moving along X-axis
    public float loopLength = 100f; // Length of the loop path

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= loopLength)
        {
            transform.position = startPosition;
        }
    }
}
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int neighboringMines = 0;
    public bool isMine = false;
    public bool isRevealed = false;
    public bool isFlagged = false;

    public void Reveal()
    {
        if (isFlagged || isRevealed) return;  // Don't reveal flagged or already revealed tiles.

        isRevealed = true;

        if (isMine)
        {
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Boom! Game Over.");
            GameManager gm = FindFirstObjectByType<GameManager>();
            gm.GameOver();
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;

            if (neighboringMines > 0)
            {
                GameObject textObj = new GameObject("MineCount");
                textObj.transform.SetParent(this.transform);
                textObj.transform.localPosition = new Vector3(0, 0f, 0);  // Slightly above tile.

                TextMesh textMesh = textObj.AddComponent<TextMesh>();
                textMesh.text = neighboringMines.ToString();
                textMesh.fontSize = 32;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.characterSize = 0.1f;
                textMesh.alignment = TextAlignment.Center;
                textMesh.transform.localScale = new Vector3(2f, 4f, 2f);
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.color = Color.blue;
                textMesh.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            else
            {
                // If no neighboring mines, auto-reveal surrounding tiles!
                RevealNeighbors();
            }
        }

        GameManager gmCheck = FindFirstObjectByType<GameManager>();
        gmCheck.CheckWinCondition();

        if (gmCheck.gameOver)
            return;
    }

    void RevealNeighbors()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.1f);
        foreach (var collider in colliders)
        {
            Tile neighbor = collider.GetComponent<Tile>();
            if (neighbor != null && !neighbor.isRevealed && !neighbor.isMine && !neighbor.isFlagged)
            {
                neighbor.Reveal();
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !isRevealed)  // Right-click to flag.
        {
            isFlagged = !isFlagged;
            GetComponent<Renderer>().material.color = isFlagged ? Color.yellow : Color.gray;
        }
    }

    private void OnMouseDown()
    {
        if (!isRevealed && !isFlagged)  // Don't allow left-click on flagged tiles.
        {
            Reveal();
        }
    }


    public void RevealMineForFPS()
    {
        if (isMine)
        {
            GetComponent<Renderer>().material.color = Color.red;
            gameObject.tag = "Mine";  // So the FPS player can detect them.
        }
        else
        {
            gameObject.SetActive(false);  // Correct way to deactivate!
        }
    }
}

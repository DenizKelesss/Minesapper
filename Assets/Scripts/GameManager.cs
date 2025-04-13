using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 10;
    public int height = 10;
    public float spacing = 1.1f;
    public bool gameOver = false;
    private Tile[,] tiles;


    public int mineCount = 10;

    void Start()
    {
        GenerateField();
    }

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("Boom! Game Over.");
    }

    public void GenerateField()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * spacing, 0, y * spacing);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                Tile tile = tileObject.GetComponent<Tile>();
                tiles[x, y] = tile;
            }
        }

        PlaceMines();
        CalculateNumbers();
    }

    void PlaceMines()
    {
        int placedMines = 0;
        System.Random rand = new System.Random();

        while (placedMines < mineCount)
        {
            int x = rand.Next(width);
            int y = rand.Next(height);

            if (!tiles[x, y].isMine)
            {
                tiles[x, y].isMine = true;
                placedMines++;
            }
        }
    }

    public void ClearField()
    {
        if (tiles != null)
        {
            foreach (Tile t in tiles)
            {
                if (t != null)
                    Destroy(t.gameObject);
            }
        }
    }

    public void CheckWinCondition()
    {
        foreach (Tile tile in tiles)
        {
            if (!tile.isMine && !tile.isRevealed)
                return;  // Still playing.
        }

        Debug.Log("You win!");
        gameOver = true;
    }

    void CalculateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].isMine) continue;

                int count = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int nx = x + i;
                        int ny = y + j;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height && tiles[nx, ny].isMine)
                        {
                            count++;
                        }
                    }
                }
                tiles[x, y].neighboringMines = count;
            }
        }
    }
}

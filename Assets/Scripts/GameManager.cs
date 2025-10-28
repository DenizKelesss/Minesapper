using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 10;
    public int height = 10;
    public float spacing = 1.1f;
    public bool gameOver = false;
    private Tile[,] tiles;

    public GameObject firstPersonPlayer;
    public GameObject mineTileFP;

    public Camera puzzleCamera;

    public int mineCount;

    //[HideInInspector]
    public int spawnedMineFPCount = 0;

    public float mineHeight = 0.10f;

    public GameObject level1Block;

    [SerializeField]
    bool canFail = false;

    void Start()
    {
        GenerateField();
    }


    public void GameOver()
    {
        if (!canFail)
            return;
        gameOver = true;
        Debug.Log("Boom! Game Over.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //invoke restart method

        if (level1Block != null)
        {
            level1Block.SetActive(false);
        }

    }

    public void GenerateField()
    {
        ClearField();  // <<< To clean the previous minefield before getting a new one.
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * spacing, mineHeight, y * spacing);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                Tile tile = tileObject.GetComponent<Tile>();
                tile.mineTileFP = this.mineTileFP;
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

    public void ActivateFirstPersonMode()
    {
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                tile.RevealMineForFPS();
            }
        }

        StartCoroutine(SwitchToFirstPerson(0.75f)); // this feels like a sweet spot

        /*
        puzzleCamera.gameObject.SetActive(false);
        firstPersonPlayer.SetActive(true);
        */
    }

    public void CheckWinCondition()
    {
        foreach (Tile tile in tiles)
        {
            if (!tile.isMine && !tile.isRevealed)
                return;
        }

        Debug.Log("You win!");
        gameOver = true;


        ActivateFirstPersonMode();
    }

    private IEnumerator SwitchToFirstPerson(float delay)
    {
        Vector3 startPos = puzzleCamera.transform.position;
        Quaternion startRot = puzzleCamera.transform.rotation;

        // firstPersonPlayer is the fpsCam holder
        Transform fpsCam = firstPersonPlayer.GetComponentInChildren<Camera>().transform;
        Vector3 targetPos = fpsCam.position;
        Quaternion targetRot = fpsCam.rotation;

        float elapsed = 0f;

        while (elapsed < delay)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / delay);

            // smooth position and rotation tranform - Lerp for pos transform, Slerp for rotation tranform
            puzzleCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            puzzleCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        puzzleCamera.gameObject.SetActive(false);
        firstPersonPlayer.SetActive(true);
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
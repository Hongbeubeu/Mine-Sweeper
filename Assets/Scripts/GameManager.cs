using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    int numFlag = 0;
    public int maxFlag;
    public int NumFlag { get => numFlag; set => numFlag = value; }

    Vector2Int[] direction = {new Vector2Int(-1, -1),
                           new Vector2Int(-1, 0),
                           new Vector2Int(-1, 1),
                           new Vector2Int(0, -1),
                           new Vector2Int(0, 1),
                           new Vector2Int(1, -1),
                           new Vector2Int(1, 0),
                           new Vector2Int(1, 1)  };
    GridGenerator gridGenerator;

    public List<List<bool>> mineMatrix = new List<List<bool>>();
    public List<List<Tile>> tileMatrix = new List<List<Tile>>();
    int row, col;
    public int point = 0;

    private void Awake()
    {
        gridGenerator = FindObjectOfType<GridGenerator>();
        gridGenerator.CreateTiles();
        maxFlag = gridGenerator.numberOfMine;
    }
    private void Start()
    {
        GenerateMines();
    }
    void GenerateMines()
    {
        row = gridGenerator.numberOfTiles / gridGenerator.tilesPerRow;
        col = gridGenerator.tilesPerRow;
        List<int> preRand = new List<int>();
        for (int i = 0; i < gridGenerator.numberOfMine; i++)
        {
            int randIndex = Random.Range(0, gridGenerator.numberOfTiles);
            while (preRand.Contains(randIndex))
            {
                randIndex = Random.Range(0, gridGenerator.numberOfTiles);
            }
            preRand.Add(randIndex);
            int y = randIndex / gridGenerator.tilesPerRow;
            int x = randIndex % gridGenerator.tilesPerRow;
            mineMatrix[y][x] = true;
            tileMatrix[y][x].hasMine = true;
            CalculateMineNearBy(y, x);
            // tileMatrix[y][x].SetActiveMine();
        }
    }

    void CalculateMineNearBy(int row, int col)
    {
        Vector2Int currentPos = new Vector2Int(col, row);
        for (int i = 0; i < direction.Length; i++)
        {
            Vector2Int checkPos = currentPos + direction[i];
            if (CheckValidPos(checkPos))
            {
                if (tileMatrix[checkPos.y][checkPos.x].hasMine)
                    continue;
                else
                    tileMatrix[checkPos.y][checkPos.x].numberOfMineNearBy++;
            }
        }
    }

    bool CheckValidPos(Vector2 pos)
    {
        if (pos.x >= 0 && pos.x < col && pos.y >= 0 && pos.y < row)
            return true;
        return false;
    }

    public void FindMineNearBy(int row, int col)
    {
        Vector2Int currentPos = new Vector2Int(col, row);
        for (int i = 0; i < direction.Length; i++)
        {
            Vector2Int checkPos = currentPos + direction[i];
            if (CheckValidPos(checkPos))
            {
                Tile tile = tileMatrix[checkPos.y][checkPos.x];
                if (tile.CurrentTileState == TileState.CHECKED)
                    continue;
                tile.OpenTile();
            }
        }
    }

    public void Lose()
    {
        for (int row = 0; row < tileMatrix.Count; row++)
        {
            for (int col = 0; col < tileMatrix[row].Count; col++)
            {
                tileMatrix[row][col].ShowResult();
            }
        }
        Debug.Log("Diem so: " + point + "/" + gridGenerator.numberOfMine);
    }

    public void IncreasePoint()
    {
        point++;
    }
}

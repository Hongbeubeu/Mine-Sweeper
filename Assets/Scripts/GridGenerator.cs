using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int numberOfTiles = 64;
    public float distanceBetweenTiles = 1.0f;
    public int tilesPerRow = 8;
    public int numberOfMine = 10;

    public void CreateTiles()
    {
        int row = numberOfTiles / tilesPerRow;
        int col = tilesPerRow;

        float xPos = -3.5f;
        float yPos = -3.5f;

        for (int y = 0; y < col; y++)
        {
            List<bool> mines = new List<bool>();
            List<Tile> tiles = new List<Tile>();
            for (int x = 0; x < row; x++)
            {
                if (x == 0)
                    xPos = -3.5f;

                GameObject tile = Instantiate(tilePrefab, new Vector2(xPos, yPos), Quaternion.identity);
                tile.name = "Tile_" + y + "_" + x;
                tile.GetComponent<Tile>().row = y;
                tile.GetComponent<Tile>().col = x;
                tile.transform.SetParent(transform);
                mines.Add(false);
                tiles.Add(tile.GetComponent<Tile>());
                xPos += 1f;
            }
            GameManager.instance.mineMatrix.Add(mines);
            GameManager.instance.tileMatrix.Add(tiles);
            yPos += 1f;
        }
    }
}

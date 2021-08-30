using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit2D.collider != null)
            {
                Tile tile = hit2D.collider.gameObject.GetComponent<Tile>();
                TileState state = tile.CurrentTileState;
                switch (state)
                {
                    case TileState.CHECKED:
                        return;

                    case TileState.UNCHECKED:
                        if (GameManager.instance.NumFlag < GameManager.instance.maxFlag)
                        {
                            tile.CurrentTileState = TileState.FLAG;
                            GameManager.instance.NumFlag++;
                        }
                        return;

                    case TileState.FLAG:
                        tile.CurrentTileState = TileState.QUESTION_MARK;
                        GameManager.instance.NumFlag--;
                        return;

                    case TileState.QUESTION_MARK:
                        tile.CurrentTileState = TileState.UNCHECKED;
                        return;
                    default:
                        break;
                }
            }
        }

    }
}

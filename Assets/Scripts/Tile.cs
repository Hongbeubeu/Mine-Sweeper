using UnityEngine;
using TMPro;

public enum TileState
{
    CHECKED,
    FLAG,
    QUESTION_MARK,
    UNCHECKED
}
public class Tile : MonoBehaviour
{
    public Sprite open, close;
    private TileState currentTileState;
    public bool hasMine, isFlag, isQuestionMark;
    public int numberOfMineNearBy;
    GameObject numberOfMineNearByObj,
               flagObj,
               questionMarkObj,
               mineObj;
    public int row;
    public int col;
    SpriteRenderer rend;

    public TileState CurrentTileState
    {
        get => currentTileState;
        set
        {
            currentTileState = value;
            switch (currentTileState)
            {
                case TileState.UNCHECKED:
                    ResetMark();
                    break;
                case TileState.FLAG:
                    ResetMark();
                    flagObj.SetActive(true);
                    break;
                case TileState.QUESTION_MARK:
                    ResetMark();
                    questionMarkObj.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private void Awake()
    {
        Init();
        CurrentTileState = TileState.UNCHECKED;
        isFlag = false;
        isQuestionMark = false;
        hasMine = false;
        numberOfMineNearBy = 0;
        rend.sprite = close;
    }

    void Init()
    {
        numberOfMineNearByObj = transform.Find("Number").gameObject;
        flagObj = transform.Find("Flag").gameObject;
        questionMarkObj = transform.Find("Question Mark").gameObject;
        mineObj = transform.Find("Mine").gameObject;

        numberOfMineNearByObj.SetActive(false);
        flagObj.SetActive(false);
        questionMarkObj.SetActive(false);
        mineObj.SetActive(false);

        rend = GetComponent<SpriteRenderer>();
    }

    void ResetMark()
    {
        flagObj.SetActive(false);
        questionMarkObj.SetActive(false);
    }

    public void SetActiveMine()
    {
        mineObj.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (CurrentTileState == TileState.CHECKED || CurrentTileState == TileState.FLAG)
            return;
        if (hasMine)
        {
            mineObj.SetActive(true);
            Debug.Log("Boommm");
            Debug.Log("You Lose");
            GameManager.instance.Lose();
        }
        else
        {
            CurrentTileState = TileState.CHECKED;
            rend.sprite = open;
            if (numberOfMineNearBy > 0)
            {
                numberOfMineNearByObj.SetActive(true);
                numberOfMineNearByObj.GetComponent<TextMeshPro>().SetText(numberOfMineNearBy.ToString());
            }
            else if (numberOfMineNearBy == 0)
            {
                GameManager.instance.FindMineNearBy(row, col);
            }
        }
    }

    public void OpenTile()
    {
        currentTileState = TileState.CHECKED;
        if (currentTileState == TileState.FLAG || currentTileState == TileState.QUESTION_MARK)
            return;

        rend.sprite = open;
        if (numberOfMineNearBy == 0)
            GameManager.instance.FindMineNearBy(row, col);
        else
        {
            numberOfMineNearByObj.GetComponent<TextMeshPro>().SetText(numberOfMineNearBy.ToString());
            numberOfMineNearByObj.SetActive(true);
        }
    }

    public void ShowResult()
    {
        if (currentTileState == TileState.FLAG && hasMine)
        {
            GameManager.instance.IncreasePoint();
            return;
        }
        else
        {
            rend.sprite = open;
            if (hasMine)
            {
                mineObj.SetActive(true);
            }
            else
            {
                if (numberOfMineNearBy > 0)
                {
                    numberOfMineNearByObj.GetComponent<TextMeshPro>().SetText(numberOfMineNearBy.ToString());
                    numberOfMineNearByObj.SetActive(true);
                }
                flagObj.SetActive(false);
                questionMarkObj.SetActive(false);
            }
        }
        currentTileState = TileState.CHECKED;
    }
}

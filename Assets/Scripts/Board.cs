using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2Int BoardSize;

    // prefabs
    public GameObject BackGroundPiece;
    public GameObject Piece;

    private GameObject[,] pieces;

    public Vector3 sizePiece;

    private bool canSwipe;
    // Start is called before the first frame update
    void Start()
    {
        pieces = new GameObject[BoardSize.x, BoardSize.y];
        var piece = Instantiate(BackGroundPiece, transform, true);
        sizePiece = piece.transform.Find("sprite").GetComponent<SpriteRenderer>().bounds.size;
        // load board
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                var bg = Instantiate(BackGroundPiece, transform, true);
                var pos = GetPiecePositionByIndex(new Vector2Int(x, y));
                pos.z = 0;
                bg.transform.localPosition = pos;
            }
        }

        InitPieces();
        SetSwipeEnabled(true);
    }

    void InitPieces()
    {
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                var piece = Instantiate(Piece, transform, true);
                var point = new Vector2Int(x, y);
                piece.GetComponent<Piece>().SetColor(Random.Range(1,6));
                piece.GetComponent<Piece>().SetPoint(point);
                var pos = GetPiecePositionByIndex(point + new Vector2Int(0,BoardSize.y));
                piece.transform.localPosition = pos;
                pieces[x,y] = piece;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPiecePositionByIndex(Vector2Int index)
    {
        const float PIECE_Z_ODER = -0.1f;
        return new Vector3(index.x * sizePiece.x, index.y * sizePiece.y, PIECE_Z_ODER);
    }

    public GameObject[,] GetPieces()
    {
        return pieces;
    }

    public bool IsPointInBoard(Vector2Int point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < BoardSize.x && point.y < BoardSize.y;
    }

    public Piece GetPieceByPoint(Vector2Int point)
    {
        return pieces[point.x, point.y].GetComponent<Piece>();
    }

    public void RemovePiece(Vector2Int point)
    {
        print("remove " + point.ToString());
        pieces[point.x, point.y] = null;
    }

    public bool CheckMatch3()
    {
        var isMatched = false;
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                var piece = pieces[x, y].GetComponent<Piece>();
                var matched = false;
                var directions = new List<Vector2Int>() {Vector2Int.right, Vector2Int.up};
                foreach (var direction in directions)
                {
                    var numPieceMatched = 1;
                    var currentPoint = piece.point + direction;
                    var listPieceMatched = new List<Piece>();
                    while (this.IsPointInBoard(currentPoint) && piece.IsSamePiece(this.GetPieceByPoint(currentPoint)))
                    {
                        numPieceMatched += 1;
                        listPieceMatched.Add(this.GetPieceByPoint(currentPoint));
                        currentPoint += direction;
                    }
                    if (numPieceMatched >= 3)
                    {
                        matched = true;
                        foreach (var p in listPieceMatched)
                        {
                            p.Match();
                        }
                    }
                }

                if (matched)
                {
                    isMatched = true;
                    piece.Match();
                }
                
            }
        }
        return isMatched;
    }

    public void SetSwipeEnabled(bool b)
    {
        this.canSwipe = b;
    }

    public void UpdateSwipeEnable()
    {
        for (var x = 0; x < BoardSize.x; x++)
        {
            for (var y = 0; y < BoardSize.y; y++)
            {
                if (pieces[x,y] != null && pieces[x, y].GetComponent<Piece>().state == PieceState.Moving)
                {
                    SetSwipeEnabled(false);
                    return;
                }
            }
        }
        SetSwipeEnabled(true);
    }

    public bool CanSwipe()
    {
        return this.canSwipe;
    }

    public void GenPieces()
    {
        print("gen pieces");
        SetSwipeEnabled(true);
        for (var x = 0; x < BoardSize.x; x++)
        {
            var numDestroyedPieces = 0;
            var y = 0;
            while (y<BoardSize.y)
            {
                var pieceObject = pieces[x, y];
                if (pieceObject == null)
                {
                    numDestroyedPieces += 1;
                }
                else
                {
                    var piece = pieceObject.GetComponent<Piece>();
                    piece.SetPoint(new Vector2Int(piece.point.x, piece.point.y - numDestroyedPieces));
                }
                y += 1;
            }

            for (var i = 0; i < numDestroyedPieces; i++)
            {
                var newPiece = Instantiate(Piece, transform, true);
                newPiece.transform.localPosition = GetPiecePositionByIndex(new Vector2Int(x, BoardSize.y + i));
                var newPieceController = newPiece.GetComponent<Piece>();
                newPieceController.SetColor(Random.Range(1,7));
                newPieceController.SetPoint(new Vector2Int(x,y - numDestroyedPieces + i));
                print("crp" + newPiece.gameObject.name + "_" + newPieceController.transform.localPosition.ToString() + "->" + newPieceController.point.ToString());
                pieces[x,y - numDestroyedPieces + i] = newPiece;
            }
        }
        // todo: check match after gen
    }
}

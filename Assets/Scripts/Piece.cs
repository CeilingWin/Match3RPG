using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int point;
    public Vector2Int oldPoint;
    private GameObject Sprite;

    private int colorId;

    private Vector3 lastMousePos;

    private Board board;
    private GameObject[,] pieces;
    // Start is called before the first frame update
    private void Awake()
    {
        Sprite = transform.Find("Sprite").gameObject;
    }

    void Start()
    {
        board = FindObjectOfType<Board>();
        pieces = board.GetPieces();
        var localPosition = transform.localPosition;
        point.x = (int) localPosition.x;
        point.y = (int) localPosition.y;
        oldPoint = point;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(int colorId)
    {
        this.colorId = colorId;
        var path = "Textures/Pieces/" + colorId;
        print(path);
        Sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }

    public int GetColor()
    {
        return this.colorId;
    }
    public void OnMouseDown()
    {
        lastMousePos = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        if (Vector3.Distance(Input.mousePosition, lastMousePos) < 25) return;
        var dir = Input.mousePosition - lastMousePos;
        var angle = Vector3.Angle(Vector3.up, dir);
        if (dir.x < 0) angle = - angle;
        var currentPoint = point;
        if (-45 <= angle && angle < 45)
        {
            currentPoint.y += 1;
        } else if (45 <= angle && angle < 135)
        {
            currentPoint.x += 1;
        } else if ((135 <= angle && angle <= 180) || (-180 <= angle && angle < -135))
        {
            currentPoint.y -= 1;
        }
        else
        {
            currentPoint.x -= 1;
        }
        OnSwipeTo(currentPoint);
    }

    void OnSwipeTo(Vector2Int point)
    {
        if (board.IsPointInBoard(point))
        {
            var swapPiece = board.GetPieceByPoint(point);
            swapPiece.SetPoint(this.point);
            this.SetPoint(point);
            if (!this.CheckMatch3() && !swapPiece.CheckMatch3())
            {
                this.SetPoint(this.oldPoint);
                swapPiece.SetPoint(swapPiece.oldPoint);
            }
        }
    }

    public void SetPoint(Vector2Int point)
    {
        this.oldPoint = this.point;
        this.point = point;
        transform.localPosition = new Vector3(point.x, point.y, transform.localPosition.z);
        pieces[point.x, point.y] = transform.gameObject;
    }

    public bool CheckMatch3()
    {
        var direction = this.point - this.oldPoint;
        List<Vector2Int> directions = new List<Vector2Int>()
        {
            Vector2Int.down,
            Vector2Int.right
        };
        List<Piece> piecesMatched = new List<Piece>() {this};
        foreach (var dir in directions)
        {
            List<Piece> listPiecesMatched = new List<Piece>();
            var cDir = dir * -1;
            var numPieceMatched = 1;
            var currentPoint = new Vector2Int(this.point.x, this.point.y) + cDir;
            while (board.IsPointInBoard(currentPoint))
            {
                var piece = board.GetPieceByPoint(currentPoint);
                if (this.IsSamePiece(piece))
                {
                    numPieceMatched += 1;
                    listPiecesMatched.Add(piece);
                }
                else
                {
                    break;
                }
                currentPoint += cDir;
            }
            currentPoint = new Vector2Int(this.point.x, this.point.y) + dir;
            while (board.IsPointInBoard(currentPoint))
            {
                var piece = board.GetPieceByPoint(currentPoint);
                if (this.IsSamePiece(piece))
                {
                    numPieceMatched += 1;
                    listPiecesMatched.Add(piece);
                }
                else
                {
                    break;
                }
                currentPoint += dir;
            }

            if (numPieceMatched >= 3)
            {
                piecesMatched = piecesMatched.Concat(listPiecesMatched).ToList();
            }
        }

        bool isMatched = piecesMatched.Count > 1;
        if (isMatched)
        {
            foreach (var piece in piecesMatched)
            {
                piece.Match();
            }
        }
        return isMatched;
    }

    public bool IsSamePiece(Piece piece)
    {
        return piece.GetColor() == this.GetColor();
    }

    public void Match()
    {
        Sprite.GetComponent<SpriteRenderer>().color = Color.black;
    }
}

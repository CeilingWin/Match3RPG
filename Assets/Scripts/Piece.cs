using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public enum PieceState
{
    Idle,
    Moving,
    Destroyed
}
public class Piece : MonoBehaviour
{
    public Vector2Int point;
    public Vector2Int oldPoint;
    private GameObject Sprite;

    private int colorId;

    private Vector3 lastMousePos;

    private Board board;
    private GameObject[,] pieces;

    public PieceState state;

    private bool isMatched;

    private bool isNeedToCheckMatch;

    private Piece swapPiece;
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
        state = PieceState.Idle;
        isMatched = false;
        isNeedToCheckMatch = false;
        swapPiece = null;
        this.gameObject.name = point.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PieceState.Idle && isMatched)
        {
            StartCoroutine(Destroy());
            return;
        }
        var desPos = new Vector3(point.x, point.y, transform.localPosition.z);
        if (Vector3.Distance(desPos, transform.localPosition) < 0.01f)
        {
            transform.localPosition = desPos;
            this.OnMoveDone();
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, desPos, 0.03f);
            state = PieceState.Moving;
        }
    }

    public void SetColor(int colorId)
    {
        this.colorId = colorId;
        var path = "Textures/Pieces/" + colorId;
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
        if (!board.CanSwipe()) return;
        if (board.IsPointInBoard(point))
        {
            board.SetSwipeEnabled(false);
            var swapPiece = board.GetPieceByPoint(point);
            swapPiece.SetPoint(this.point);
            this.swapPiece = swapPiece;
            this.SetPoint(point, true);
        }
    }

    void OnMoveDone()
    {
        state = PieceState.Idle;
        if (isNeedToCheckMatch)
        {
            board.CheckMatch3();
            if (!this.IsMatched())
            {
                this.SetPoint(this.oldPoint);
                this.swapPiece.SetPoint(swapPiece.oldPoint);
            }
            isNeedToCheckMatch = false;
        }
    }

    private void SetPoint(Vector2Int point, bool needToCheckMatch = false)
    {
        this.oldPoint = this.point;
        this.point = point;
        pieces[point.x, point.y] = transform.gameObject;
        state = PieceState.Moving;
        this.isNeedToCheckMatch = needToCheckMatch;
        this.gameObject.name = point.ToString();
    }

    public bool IsMatched3()
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
        this.isMatched = true;
    }

    IEnumerator Destroy()
    {
        Sprite.GetComponent<SpriteRenderer>().color = Color.black;
        state = PieceState.Destroyed;
        yield return new WaitForSeconds(0.5f);
        board.RemovePiece(point);
        if (swapPiece != null)
        {
            board.GenPieces();
        }
        GameObject.Destroy(this.gameObject);
        yield return null;
    }

    public bool IsMatched()
    {
        return isMatched;
    }
}

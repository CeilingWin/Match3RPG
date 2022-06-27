using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int point;
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
            var swapPiece = pieces[point.x, point.y];
            swapPiece.GetComponent<Piece>().SetPoint(this.point);
            this.SetPoint(point);
        }
    }

    public void SetPoint(Vector2Int point)
    {
        this.point = point;
        transform.localPosition = new Vector3(point.x, point.y, transform.localPosition.z);
        pieces[point.x, point.y] = transform.gameObject;
    }
}

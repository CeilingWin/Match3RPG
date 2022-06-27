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
                print(x + " " + y);
                var bg = Instantiate(BackGroundPiece, transform, true);
                bg.transform.localPosition = GetPiecePositionByIndex(new Vector2Int(x, y));
            }
        }
        InitPieces();
    }

    void InitPieces()
    {
        const float PIECE_Z_ODER = -0.1f;
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                var piece = Instantiate(Piece, transform, true);                
                var pos = GetPiecePositionByIndex(new Vector2Int(x, y));
                pos.z = PIECE_Z_ODER;
                piece.transform.localPosition = pos;
                piece.GetComponent<Piece>().SetColor(Random.Range(1,7));
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
        return new Vector3(index.x * sizePiece.x, index.y * sizePiece.y, 0);
    }

    public GameObject[,] GetPieces()
    {
        return pieces;
    }

    public bool IsPointInBoard(Vector2Int point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < BoardSize.x && point.y < BoardSize.y;
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using InputControl;
using Match3;
using Rpg;
using Unit;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;

    public float ItemSize;
    [FormerlySerializedAs("inputHandler")] [SerializeField]
    private InputHandler InputHandler;

    private Match3Module match3Module;
    private RpgModule rpgModule;
    private AsyncLazy currentTask;
    private CancellationToken cancellationToken;
    
    // handle input
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        cancellationToken = this.GetCancellationTokenOnDestroy();
        match3Module = new Match3Module(0, ItemPrefab);
        rpgModule = new RpgModule();
        currentTask = Init(cancellationToken).ToAsyncLazy();
        InputHandler.TouchBegan += OnTouchBegan;
        InputHandler.TouchMoved += OnTouchMoved;
        InputHandler.TouchEnded += OnTouchEnded;
        InputHandler.TouchCanceled += OnTouchCancel;
    }

    private async UniTask Init(CancellationToken cancellationToken)
    {
        await match3Module.Init(cancellationToken);
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    
    // private bool 

    private void OnTouchBegan(object sender, Vector3 boardPosition)
    {
        startPosition = boardPosition;
        // Debug.Log("began: " + match3Module.PositionToGridPosition(startPosition));
    }
    
    private void OnTouchMoved(object sender, Vector3 boardPosition)
    {
        
        // Debug.Log("move: " +match3Module.PositionToGridPosition(boardPosition));
        if (!CanControlGame()) return;
        if (Vector3.Distance(boardPosition, startPosition) > GameConst.MinTouchDistance)
        {
            if (match3Module.IsPointOnItem(startPosition))
            {
                currentTask = MoveItem(startPosition, boardPosition, cancellationToken).ToAsyncLazy();
            }
        }
    }
    
    private void OnTouchEnded(object sender, Vector3 boardPosition)
    {
        // Debug.Log("end: " +match3Module.PositionToGridPosition(boardPosition) + " with stp" + match3Module.PositionToGridPosition(startPosition));
        // Debug.Log("dir" + match3Module.GetMoveDirection(startPosition, boardPosition));
        if (!CanControlGame()) return;
        if (Vector3.Distance(boardPosition, startPosition) < GameConst.MinTouchDistance)
        {
            Debug.Log("click");
        }
        else
        {
        }
    }
    
    private void OnTouchCancel(object sender, EventArgs e)
    {
        Debug.Log("cancel: ");
    }

    private async UniTask MoveItem(Vector3 startPosition, Vector3 endPosition, CancellationToken cancellationToken)
    {
        var startGridPos = match3Module.PositionToGridPosition(startPosition);
        var dir = match3Module.GetMoveDirection(startPosition, endPosition);
        var endGridPos = startGridPos + dir;
        await match3Module.Swap(startGridPos, endGridPos, cancellationToken);
    }

    private bool CanControlGame()
    {
        return currentTask == null || currentTask.Task.Status.IsCompleted();
    }
}
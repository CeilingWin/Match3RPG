using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using InputControl;
using Match3;
using Rpg;
using Unit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public static Game instance;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] public GameObject YourBase;

    [SerializeField]
    private InputHandler InputHandler;

    public Match3Module Match3Module { get; private set; }
    public RpgModule RpgModule { get; private set; }
    private AsyncLazy currentTask;
    private CancellationToken cancellationToken;
    
    // game state
    private GameState gameState;
    private int NumWave = 4;

    // handle input
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        cancellationToken = this.GetCancellationTokenOnDestroy();
        Match3Module = new Match3Module(0, ItemPrefab);
        RpgModule = new RpgModule();
        InputHandler.TouchBegan += OnTouchBegan;
        InputHandler.TouchMoved += OnTouchMoved;
        InputHandler.TouchEnded += OnTouchEnded;
        InputHandler.TouchCanceled += OnTouchCancel;
        gameState = new GameState();
        currentTask = Init(cancellationToken).ToAsyncLazy();
    }

    private async UniTask Init(CancellationToken cancellationToken)
    {
        await Match3Module.Init(cancellationToken);
        await RpgModule.Init(cancellationToken);
        await StartGame();
    }

    private async UniTask StartGame()
    {
        await UniTask.CompletedTask;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCompletedTask()) return;
        // todo: check end game first
        // update game
        switch (gameState.GetCurrentPhase())
        {
            case GamePhase.GenerateMonster:
                currentTask = RpgModule.GenerateMonster(cancellationToken).ToAsyncLazy();
                gameState.SetPhase(GamePhase.PlayerMove);
                break;
            case GamePhase.PlayerMove:
                if (gameState.GetNumberMoveRemain() <= 0)
                {
                    gameState.SetPhase(GamePhase.MachineAttack);
                    currentTask = RpgModule.LetMachinesAttack(cancellationToken).ToAsyncLazy();
                }
                break;
            case GamePhase.MachineAttack:
                gameState.SetPhase(GamePhase.MonsterAttack);
                currentTask = RpgModule.LetMonstersAttack(cancellationToken).ToAsyncLazy();
                break;
            case GamePhase.MonsterAttack:
                NextWave();
                break;
            case GamePhase.Ended:
                // todo:
                break;
        }
    }

    private void NextWave()
    {
        if (gameState.GetWave() > NumWave)
        {
            gameState.SetPhase(GamePhase.Ended);
        }
        else
        {
            gameState.IncreaseWave();
            gameState.SetPhase(GamePhase.GenerateMonster);
        }
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
            if (Match3Module.IsPointOnItem(startPosition))
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
    }

    private async UniTask MoveItem(Vector3 startPosition, Vector3 endPosition, CancellationToken cancellationToken)
    {
        var startGridPos = Match3Module.PositionToGridPosition(startPosition);
        var dir = Match3Module.GetMoveDirection(startPosition, endPosition);
        var endGridPos = startGridPos + dir;
        gameState.DecreaseNumMove();
        await Match3Module.Swap(startGridPos, endGridPos, cancellationToken);
    }

    private bool CanControlGame()
    {
        return IsCompletedTask() 
               && gameState.GetCurrentPhase() == GamePhase.PlayerMove
               && gameState.GetNumberMoveRemain() > 0;
    }

    private bool IsCompletedTask()
    {
        return currentTask == null || currentTask.Task.Status.IsCompleted();
    }

    private void OnDestroy()
    {
        InputHandler.TouchBegan -= OnTouchBegan;
        InputHandler.TouchMoved -= OnTouchMoved;
        InputHandler.TouchEnded -= OnTouchEnded;
        InputHandler.TouchCanceled -= OnTouchCancel;
    }
}
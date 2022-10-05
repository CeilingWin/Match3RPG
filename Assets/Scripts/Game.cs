using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using InputControl;
using Match3;
using Popup;
using Rpg;
using Rpg.Ability;
using Rpg.Units;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] public GameObject YourBase;

    [SerializeField] private InputHandler InputHandler;

    [SerializeField] private GameUI GameUI;

    public Match3Module Match3Module { get; private set; }
    public RpgModule RpgModule { get; private set; }
    private AsyncLazy currentTask;
    private CancellationToken cancellationToken;

    // game state
    private GameState gameState;
    private int numWave = 3;
    private Machine selectedUnit;

    // handle input
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        ItemPool.GetIns().Clear();
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
        GameUI.UpdateStates();
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
        if (gameState.GetCurrentPhase() != GamePhase.Ended && RpgModule.GetYourBase().IsDied())
        {
            YouLose();
            gameState.SetPhase(GamePhase.Ended);
            return;
        }
        // update game
        switch (gameState.GetCurrentPhase())
        {
            case GamePhase.BeginNewWave:
                OnNewWave();
                break;
            case GamePhase.GenerateMonster:
                GameUI.UpdateStates();
                currentTask = RpgModule.GenerateMonster(gameState.GetCurrentTurn(), cancellationToken).ToAsyncLazy();
                gameState.SetPhase(GamePhase.PlayerMove);
                break;
            case GamePhase.PlayerMove:
                if (gameState.GetNumberMoveRemain() <= 0)
                {
                    gameState.SetPhase(GamePhase.MachineAttack);
                }
                break;
            case GamePhase.MachineAttack:
                currentTask = LetMachinesAttack(cancellationToken).ToAsyncLazy();
                break;
            case GamePhase.MonsterAttack:
                currentTask = LetMonstersAttack(cancellationToken).ToAsyncLazy();
                break;
            case GamePhase.Ended:
                // todo:
                break;
        }
    }

    private async UniTask LetMachinesAttack(CancellationToken cancellationToken)
    {
        await RpgModule.LetMachinesAttack(cancellationToken);
        gameState.SetPhase(GamePhase.MonsterAttack);
    }

    private async UniTask LetMonstersAttack(CancellationToken cancellationToken)
    {
        await RpgModule.LetMonstersAttack(cancellationToken);
        await RpgModule.UpdateAllUnits();

        if (RpgModule.IsGenAllMonster() && RpgModule.GetNumMonster() == 0)
        {
            gameState.SetPhase(GamePhase.BeginNewWave);
        }
        else
        {
            gameState.NextTurn();
            gameState.SetPhase(GamePhase.GenerateMonster);
        }
    }

    private void OnNewWave()
    {
        gameState.IncreaseWave();
        if (gameState.GetWave() > numWave)
        {
            YouWin();
            gameState.SetPhase(GamePhase.Ended);
        }
        else
        {
            GameUI.ShowNotification("Wave " + gameState.GetWave());
            RpgModule.InitMonstersOfWave(gameState.GetWave());
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
            if (Match3Module.IsPointOnItem(startPosition) && selectedUnit == null)
            {
                currentTask = MoveItem(startPosition, boardPosition, cancellationToken).ToAsyncLazy();
            }
        }
    }

    private void OnTouchEnded(object sender, Vector3 boardPosition)
    {
        if (!CanControlGame()) return;
        if (Vector3.Distance(boardPosition, startPosition) < GameConst.MinTouchDistance)
        {
            if (gameState.GetCurrentPhase() != GamePhase.PlayerMove) return;
            var gridPosition = Match3Module.PositionToGridPosition(boardPosition);
            if (selectedUnit == null)
            {
                var unit = RpgModule.GetMachine(gridPosition);
                if (!unit) return;
                if (!unit.CanMove()) return;
                selectedUnit = unit;
                RpgModule.ShowMoveAbleArea(selectedUnit);
            }
            else
            {
                if (selectedUnit.CanMoveTo(gridPosition))
                {
                    currentTask = selectedUnit.GetComponent<Move>().MoveTo(gridPosition).ToAsyncLazy();
                    gameState.DecreaseNumMove();
                }
                else
                {
                    Debug.Log("cancel move machine");
                }

                RpgModule.HideMoveAbleArea();
                selectedUnit = null;
            }
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
        if (!Match3Module.CanSwap(startGridPos, endGridPos)) return;
        gameState.DecreaseNumMove();
        await Match3Module.Swap(startGridPos, endGridPos, cancellationToken);
    }

    private async UniTask MoveUnit(Vector3 desPosition)
    {
        await new UniTask();
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

    public GameState GetState()
    {
        return gameState;
    }

    public int GetNumberWave()
    {
        return numWave;
    }

    private void YouLose()
    {
        PopupGameLose.Create();
    }

    private void YouWin()
    {
        PopupGameWin.Create();
    }

    private void OnDestroy()
    {
        InputHandler.TouchBegan -= OnTouchBegan;
        InputHandler.TouchMoved -= OnTouchMoved;
        InputHandler.TouchEnded -= OnTouchEnded;
        InputHandler.TouchCanceled -= OnTouchCancel;
    }

    public void Destroy()
    {
        ItemPool.GetIns().Clear();
    }
}
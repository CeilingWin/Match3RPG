using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.FillStrategies;
using Match3.Solver;
using Unit;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Match3
{
    public class Match3Module
    {
        private readonly float itemSize;


        private readonly GameObject itemPrefab;
        private readonly Math3Board gameBoard;
        private readonly IFillStrategy fillStrategy;
        private readonly IMatch3Solver solver;

        public Match3Module(int level, GameObject itemPrefab)
        {
            // todo: load config
            this.itemPrefab = itemPrefab;
            this.itemSize = 1.1f;
            gameBoard = new Math3Board(8, 8);
            fillStrategy = new FadeInStrategy(this);
            solver = new SequenceSolver();
        }

        public async UniTask Init(CancellationToken cancellationToken)
        {
            await fillStrategy.Fill(gameBoard, cancellationToken);
        }
        public Vector3 IndexToWorldPosition(int row, int column)
        {
            float z = (-gameBoard.rowCount / 2f + row + 0.5f) * itemSize;
            float x = (-gameBoard.columnCount / 2f + column + 0.5f) * itemSize;
            return new Vector3(x, 0, z);
        }

        public bool IsPointOnItem(Vector3 position)
        {
            var pos = PositionToGridPosition(position);
            return gameBoard.IsPointInBoard(pos);
        }

        public GridPosition PositionToGridPosition(Vector3 position)
        {
            int column = (int)Math.Round(position.x / itemSize - (-gameBoard.columnCount / 2f + 0.5f));
            int row = (int) Math.Round(position.z / itemSize - (-gameBoard.rowCount / 2f + 0.5f));
            return new GridPosition(row, column);
        }

        public GridPosition GetMoveDirection(Vector3 startPos, Vector3 endPos)
        {
            var dx = endPos.x - startPos.x;
            var dz = endPos.z - startPos.z;
            if (Math.Abs(dx) > Math.Abs(dz))
            {
                if (dx > 0) return GridPosition.Right;
                return GridPosition.Left;
            }
            else
            {
                if (dz > 0) return GridPosition.Up;
                return GridPosition.Down;
            }
        }

        public async UniTask Swap(GridPosition startPos, GridPosition endPos, CancellationToken cancellationToken)
        {
            if (!gameBoard.IsPointInBoard(startPos) || !gameBoard.IsPointInBoard(endPos)) return;
            var slot1 = gameBoard.Slots[startPos.RowIndex, startPos.ColumnIndex];
            var slot2 = gameBoard.Slots[endPos.RowIndex, endPos.ColumnIndex];
            Debug.Log("swap" + startPos + " to " + endPos);
            if (!slot1.GetState().CanContainItem() || !slot2.GetState().CanContainItem())
            {
                Debug.Log("have slot cant contain item");
                return;
            }

            if (slot1.GetItem() == null || slot2.GetItem() == null)
            {
                Debug.Log("dont have item to swap");
                return;
            }
            await DoAnimateSwap(slot1, slot2, cancellationToken);
            var list = new List<GridPosition>() {startPos, endPos};
            await SolveBoard(list, cancellationToken);
        }

        private async UniTask SolveBoard(List<GridPosition> gridPositions, CancellationToken cancellationToken)
        {
            var solveData = solver.SolveBoard(gameBoard, gridPositions);
            await fillStrategy.Solve(gameBoard, solveData, cancellationToken);
            if (solveData.SolvedSlot.Count > 0)
            {
                List<GridPosition> solvedGridPos = new List<GridPosition>();
                for (var row = 0; row < gameBoard.rowCount; row++)
                {
                    for (var column = 0; column < gameBoard.columnCount; column++)
                    {
                        if (solveData.SolvedSlot.Contains(gameBoard.Slots[row, column])) 
                            solvedGridPos.Add(new GridPosition(row, column));
                    }
                }

                // await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cancellationToken);
                await SolveBoard(solvedGridPos, cancellationToken);
            }
        }

        private async UniTask DoAnimateSwap(ISlot slot1, ISlot slot2, CancellationToken cancellationToken)
        {
            var item1 = slot1.GetItem();
            var item2 = slot2.GetItem();
            var seq = DOTween.Sequence();
            var desPosItem1 = item2.GetPosition();
            var desPosItem2 = item1.GetPosition();
            var task1 = item1.Transform.DOMove(desPosItem1, 0.4f).WithCancellation(cancellationToken);
            var task2 = item2.Transform.DOMove(desPosItem2, 0.4f).WithCancellation(cancellationToken);
            slot1.SetItem(item2);
            slot2.SetItem(item1);
            await UniTask.WhenAll(task1, task2);
        }
    }
}
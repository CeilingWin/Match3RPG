using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Match3.FillStrategies;
using Match3.Solver;
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
            var slot1 = gameBoard.Slots[startPos.RowIndex, startPos.ColumnIndex];
            var slot2 = gameBoard.Slots[endPos.RowIndex, endPos.ColumnIndex];
            if (!slot1.GetState().CanContainItem() || !slot2.GetState().CanContainItem())
            {
                Debug.Log("can move item");
                return;
            }

            var list = new List<GridPosition>() {startPos, endPos};
            var solveData = solver.SolveBoard(gameBoard, list);
            await fillStrategy.Solve(gameBoard, solveData, cancellationToken);
        }
    }
}
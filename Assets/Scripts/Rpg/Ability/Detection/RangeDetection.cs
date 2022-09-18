using System;
using System.Collections.Generic;
using Match3;
using UnityEngine;

namespace Rpg.Ability.Detection
{
    public class RangeDetection : MonoBehaviour, IDetectAbility
    {
        private int range;

        public void SetRange(int range)
        {
            this.range = range;
        }

        public List<Units.Unit> DetectAllUnits<T>() where T : Units.Unit
        {
            var boardSize = Game.instance.Match3Module.GetBoardSize();
            var currentGridPos = GetComponent<Units.Unit>().GetGridPosition();
            var units = new List<Units.Unit>();
            var rpgModule = Game.instance.RpgModule;
            var startRow = Math.Max(currentGridPos.RowIndex - range, 0);
            var endRow = Math.Min(currentGridPos.RowIndex + range, boardSize.RowIndex);
            var startColumn = Math.Max(currentGridPos.ColumnIndex - range, 0);
            var endColumn = Math.Min(currentGridPos.ColumnIndex + range, boardSize.ColumnIndex);
            for (var rowIndex = startRow; rowIndex < endRow; rowIndex++)
            {
                for (var columnIndex = startColumn; columnIndex < endColumn; columnIndex++)
                {
                    var gridPosition = new GridPosition(rowIndex, columnIndex);
                    if (gridPosition == currentGridPos) continue;
                    if (GridPosition.Distance(gridPosition, currentGridPos) <= range)
                    {
                        var unit = rpgModule.GetUnit<T>(gridPosition);
                        if (unit) units.Add(unit);
                    }
                }
            }

            return units;
        }

        public int GetRange()
        {
            return range;
        }
    }
}
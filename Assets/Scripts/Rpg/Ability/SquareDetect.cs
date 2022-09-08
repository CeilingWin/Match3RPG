using System.Collections.Generic;
using Match3;
using UnityEngine;

namespace Rpg.Ability
{
    public class SquareDetect : MonoBehaviour, IDetectAbility
    {
        private static readonly List<GridPosition> directions = new List<GridPosition>()
        {
            GridPosition.Down, GridPosition.Left, GridPosition.Right, GridPosition.Up
        };

        public List<Units.Unit> DetectAllUnits<T>() where T : Units.Unit
        {
            var currentGridPos = GetComponent<Units.Unit>().GetGridPosition();
            var units = new List<Units.Unit>();
            var rpgModule = Game.instance.RpgModule;
            foreach (var direction in directions)
            {
                var gridPosition = currentGridPos + direction;
                var unit = rpgModule.GetUnit<T>(gridPosition);
                if (unit) units.Add(unit);
            }
            return units;
        }
    }
}
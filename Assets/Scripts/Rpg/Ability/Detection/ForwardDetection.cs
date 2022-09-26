using System.Collections.Generic;
using Match3;
using UnityEngine;

namespace Rpg.Ability.Detection
{
    public class ForwardDetection : MonoBehaviour, IDetectAbility
    {
        private GridPosition forward;
        public void SetForwardDirection(GridPosition direction)
        {
            forward = direction;
        }
        public List<Units.Unit> DetectAllUnits<T>() where T : Units.Unit
        {
            var currentGridPos = GetComponent<Units.Unit>().GetGridPosition();
            var units = new List<Units.Unit>();
            var rpgModule = Game.instance.RpgModule;
            var unit = rpgModule.GetUnit<T>(currentGridPos + forward);
            if (unit) units.Add(unit);
            return units;
        }

        public int GetRange()
        {
            return 1;
        }
    }
}
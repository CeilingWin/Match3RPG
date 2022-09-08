using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Match3;
using Rpg.Ability;
using Unity.VisualScripting;
using UnityEngine;

namespace Rpg.Units
{
    public class Machine : Unit
    {
        new void Start()
        {
            base.Start();
            this.AddComponent<Attack>();
        }

        public override UniTask Attack()
        {
            throw new System.NotImplementedException();
        }

        public override UniTask Die()
        {
            throw new System.NotImplementedException();
        }

        public bool CanMove()
        {
            return GetComponent<Stat>().GetSpeed() > 0;
        }

        public bool CanMoveTo(GridPosition gridPosition)
        {
            var currentPos = GetGridPosition();
            var distance = GridPosition.Distance(currentPos, gridPosition);
            var machineAtPos = Game.instance.RpgModule.GetMachine(gridPosition);
            var slot = Game.instance.Match3Module.GameBoard.GetSlot(gridPosition);
            return distance <= GetComponent<Stat>().GetSpeed() 
                   && currentPos != gridPosition
                   && machineAtPos == null
                   && (slot != null && slot.CanPutMachine());
        }
    }
}
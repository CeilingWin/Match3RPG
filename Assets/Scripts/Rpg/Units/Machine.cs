using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Match3;
using Rpg.Ability;
using Rpg.Ability.Detection;
using Unity.VisualScripting;
using UnityEngine;

namespace Rpg.Units
{
    public abstract class Machine : Unit
    {
        protected override void Start()
        {
            base.Start();
            this.AddComponent<Attack>();
        }

        public override async UniTask Attack()
        {
            var monsters = GetComponent<IDetectAbility>().DetectAllUnits<Monster>();
            if (monsters.Count == 0) return;
            var target = monsters[0];
            if (!target) return;
            Debug.Log("attack " + target);
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            await GetComponent<Attack>().DoAttack();
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
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
            var unit = Game.instance.RpgModule.GetUnit<Unit>(gridPosition);
            var slot = Game.instance.Match3Module.GameBoard.GetSlot(gridPosition);
            return distance <= GetComponent<Stat>().GetSpeed()
                   && currentPos != gridPosition
                   && unit == null
                   && (slot != null && slot.CanPutUnit());
        }
    }
}
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Match3;
using Rpg.Ability;
using Rpg.Ability.Detection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;
using Material = Enum.Material;

namespace Rpg.Units
{
    public abstract class Machine : Unit, IComparable<Machine>
    {
        protected Material material;
        protected override void Start()
        {
            base.Start();
            this.AddComponent<Attack>();
        }

        public override async UniTask Attack()
        {
            var monsters = GetComponent<IDetectAbility>().DetectAllUnits<Monster>();
            if (monsters.Count == 0) return;
            var monsterOrder = new MonsterOrder(GetGridPosition());
            monsters.Sort(monsterOrder);
            var target = monsters[0];
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

        public Material GetMaterial()
        {
            return material;
        }

        public int CompareTo(Machine other)
        {
            var xIndex = ArrayUtility.IndexOf(CompareUtils.Materials, this.GetMaterial());
            var yIndex = ArrayUtility.IndexOf(CompareUtils.Materials, other.GetMaterial());
            return xIndex - yIndex;
        }
    }
}
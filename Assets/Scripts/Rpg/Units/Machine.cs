using System;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Ability.Detection;
using Unity.VisualScripting;
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

        public override async UniTask Die()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            Destroy(gameObject);
            await UniTask.CompletedTask;
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
            var xIndex = Array.IndexOf(CompareUtils.Materials, this.GetMaterial());
            var yIndex = Array.IndexOf(CompareUtils.Materials, other.GetMaterial());
            return xIndex - yIndex;
        }

        public override bool IsDied()
        {
            var stat = GetComponent<Stat>();
            return stat.GetHp() > 0 && stat.GetCountDown() > 0;
        }
    }
}
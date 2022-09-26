using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
            defaultDirection = Vector3.forward;
            base.Start();
        }

        public override async UniTask Attack()
        {
            await GetComponent<IAttack>().Attack<Monster>();
        }
        
        public override void SortUnits(List<Unit> units)
        {
            var monsterOrder = new MonsterOrder(GetGridPosition());
            units.Sort(monsterOrder);
        }

        public override async UniTask Die()
        {
            Game.instance.RpgModule.OnMachineDied(this);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));
            await transform.DOScale(Vector3.zero, 0.6f).WithCancellation(this.GetCancellationTokenOnDestroy());
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
            return stat.GetHp() <= 0 || stat.GetCountDown() <= 0;
        }
        
        public override async UniTask TakeDamage(int damage)
        {
            var stat = GetComponent<Stat>();
            stat.ChangeHp(-damage);
            if (stat.GetHp() == 0)
            {
                await Die();
            }
            else
            {
                
            }
        }

        public override async UniTask UpdateUnit()
        {
            base.UpdateUnit();
            var stat = GetComponent<Stat>();
            stat.ChangeCountDown(-1);
            if (stat.GetCountDown() == 0)
            {
                await Die();
            }
        }
    }
}
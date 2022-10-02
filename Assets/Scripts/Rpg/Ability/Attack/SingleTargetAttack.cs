using System;
using Cysharp.Threading.Tasks;
using Rpg.Ability.Detection;
using UnityEngine;

namespace Rpg.Ability.Attack
{
    public class SingleTargetAttack : BaseAttack, IAttack
    {
        public async UniTask Attack<T>() where T : Units.Unit
        {
            var units = GetComponent<IDetectAbility>().DetectAllUnits<T>();
            if (units.Count == 0) return;
            var thisUnit = GetComponent<Units.Unit>();
            thisUnit.SortUnits(units);
            var target = units[0];
            await Attack(target);
            transform.rotation = Quaternion.LookRotation(thisUnit.defaultDirection);
        }

        public async UniTask Attack<T>(T target) where T : Units.Unit
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var taskAttack = DoAttack();
            await UniTask.Delay(TimeSpan.FromSeconds(GetComponent<Units.Unit>().delayAttack));
            PerformAttackToTarget(target);
            await taskAttack;
        }
    }
}
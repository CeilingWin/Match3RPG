using System;
using Cysharp.Threading.Tasks;
using Rpg.Ability.Detection;
using UnityEngine;

namespace Rpg.Ability.Attack
{
    public class MultiTargetAttack : BaseAttack, IAttack
    {
        public async UniTask Attack<T>() where T : Units.Unit
        {
            var units = GetComponent<IDetectAbility>().DetectAllUnits<T>();
            if (units.Count == 0) return;
            var thisUnit = GetComponent<Units.Unit>();
            thisUnit.SortUnits(units);
            var taskAttack = DoAttack();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            foreach (var unit in units)
            {
                PerformAttackToTarget(unit);
            }
            await taskAttack;
            transform.rotation = Quaternion.LookRotation(thisUnit.defaultDirection);
        }
    }
}
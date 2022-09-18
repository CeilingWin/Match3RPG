﻿using System;
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
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var taskAttack = DoAttack();
            await UniTask.Delay(TimeSpan.FromSeconds(thisUnit.delayAttack));
            PerformAttackToTarget(target);
            await taskAttack;
            transform.rotation = Quaternion.LookRotation(thisUnit.defaultDirection);
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Rpg.Units.Machines;
using UnityEngine;

namespace Rpg.Ability.Attack
{
    public class ParalyzerAttack : SingleTargetAttack
    {
        private Units.Unit target;
        public override async UniTask DoAttack()
        {
            var controller = GetComponent<Paralyzer>();
            var weapon = controller.weapon;
            var startPos = weapon.transform.position;
            var desPos = target.transform.position;
            desPos.y = 0.4f;
            var attackJob = base.DoAttack();
            await UniTask.Delay(TimeSpan.FromSeconds(controller.delayAttack));
            var amor = Instantiate(controller.amorVfx);
            amor.transform.position = weapon.transform.position;
            amor.transform.rotation = Quaternion.LookRotation(desPos - startPos);
            var seq = DOTween.Sequence();
            var time = Vector3.Distance(startPos, desPos) / 4;
            seq.Append(amor.transform.DOMove(desPos, time));
            GameObject explosion;
            seq.AppendCallback(() =>
            {
                explosion = Instantiate(controller.explosionVfx);
                explosion.transform.position = desPos;
                Destroy(amor);
            });
            var amorJob = seq.WithCancellation(controller.GetCancellationTokenOnDestroy());
            await UniTask.WhenAll(attackJob, amorJob);
        }

        public override UniTask Attack<T>(T target)
        {
            this.target = target;
            return base.Attack(target);
        }
    }
}
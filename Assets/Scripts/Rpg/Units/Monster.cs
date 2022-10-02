using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3;
using Rpg.Ability;
using UnityEngine;
using Utils;

namespace Rpg.Units
{
    public abstract class Monster : Unit
    {
        protected override void Start()
        {
            defaultDirection = Vector3.back;
            base.Start();
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }

        public override async UniTask Attack()
        {
            var stat = GetComponent<Stat>();
            var currentPosition = GetGridPosition();
            if (currentPosition.RowIndex == 0)
            {
                await GetComponent<BaseAttack>().DoAttack();
                Game.instance.RpgModule.GetYourBase().TakeDamage(stat.GetDamage());
            }
            else
            {
                var forwardPosition = currentPosition + GridPosition.Down;
                var target = Game.instance.RpgModule.GetMachine(forwardPosition);
                if (target)
                {
                    var attackJob = GetComponent<BaseAttack>().DoAttack();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                    await target.TakeDamage(stat.GetDamage());
                    await attackJob;
                }
                else
                {
                    List<GridPosition> directions = new List<GridPosition>()
                    {
                        GridPosition.Down,
                        GridPosition.Left,
                        GridPosition.Right
                    };
                    foreach (var direction in directions)
                    {
                        var pos = currentPosition + direction;
                        if (Game.instance.Match3Module.CanPutUnit(pos)
                            && Game.instance.RpgModule.CanPutUnit(pos))
                        {
                            await GetComponent<Move>().MoveTo(pos);
                            break;
                        }
                    }
                    await GetComponent<IAttack>().Attack<Machine>();
                }
            }
        }

        public override async UniTask Die()
        {
            Game.instance.RpgModule.OnMonsterDied(this);
            var animator = GetComponent<Animator>();
            await AnimatorUtils.PlayAnimationSync(animator, "Die", "Idle");
            await transform.DOScale(Vector3.zero, 0.2f).WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public override bool IsDied()
        {
            return GetComponent<Stat>().GetHp() <= 0;
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
                var animator = GetComponent<Animator>();
                await AnimatorUtils.PlayAnimationSync(animator, "GetHit", "Idle");
            }
        }

        public override void SortUnits(List<Unit> units)
        {
            var machineOrder = new MachineOrder(GetGridPosition());
            units.Sort(machineOrder);
        }

        public async UniTask AttackPlayerBase()
        {
            await GetComponent<BaseAttack>().DoAttack();
            var damage = GetComponent<Stat>().GetDamage();
            Game.instance.RpgModule.GetYourBase().TakeDamage(damage);
        }
    }
}
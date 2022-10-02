using System;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Unity.VisualScripting;
using UnityEngine;

namespace Rpg.Units.Monsters.MonsterBehaviour
{
    public class BallistaAttack : MonoBehaviour
    {
        void Start()
        {
            var pathFinding = this.AddComponent<PathFinding>();
            pathFinding.SetCheckCanMoveFunc(position =>
            {
                var canPutUnit = Game.instance.Match3Module.CanPutUnit(position);
                var isHaveMonster = Game.instance.RpgModule.GetUnit<Monster>(position) != null;
                return !isHaveMonster && canPutUnit;
            });
            pathFinding.SetCheckTargetFunc(position => position.RowIndex == 0);
        }

        public async UniTask Attack()
        {
            var currentPosition = GetComponent<Monster>().GetGridPosition();
            var stat = GetComponent<Stat>();
            if (currentPosition.RowIndex == 0)
            {
                await GetComponent<BaseAttack>().DoAttack();
                Game.instance.RpgModule.GetYourBase().TakeDamage(stat.GetDamage());
            }
            else
            {
                var forwardPosition = currentPosition + GridPosition.Down;
                var target = Game.instance.RpgModule.GetMachine(forwardPosition);
                if (target) await Attack(target);
                else
                {
                    var path = GetComponent<PathFinding>().FindPath(currentPosition);
                    if (path == null) return;
                    var numMoveRemain = stat.GetSpeed();
                    foreach (var nextPosition in path)
                    {
                        if (numMoveRemain == 0) break;
                        var machine = Game.instance.RpgModule.GetMachine(nextPosition);
                        if (machine)
                        {
                            await Attack(machine);
                            break;
                        }
                        numMoveRemain--;
                        await GetComponent<Move>().MoveTo(nextPosition);
                    }

                    if (numMoveRemain > 0 && GetComponent<Monster>().GetGridPosition().RowIndex == 0)
                    {
                        await GetComponent<BaseAttack>().DoAttack();
                        Game.instance.RpgModule.GetYourBase().TakeDamage(stat.GetDamage());
                    }
                }
            }
        }

        private async UniTask Attack(Machine target)
        {
            var attackJob = GetComponent<BaseAttack>().DoAttack();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            await target.TakeDamage(GetComponent<Stat>().GetDamage());
            await attackJob;
        }
    }
}
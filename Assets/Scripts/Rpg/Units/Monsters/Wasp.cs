using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Ability.Attack;
using Rpg.Ability.Detection;
using Rpg.Units.Common;
using Unity.VisualScripting;
using Utils;

namespace Rpg.Units.Monsters
{
    public class Wasp : Monster
    {
        protected override void Start()
        {
            delayAttack = 1.2f;
            base.Start();
            GetComponent<Stat>().SetStat(3, 2, 1, 3, Int32.MaxValue);
            this.AddComponent<SingleTargetAttack>();
            this.AddComponent<SquareDetection>();

            var game = Game.instance;
            var pathFinding = this.AddComponent<PathFinding>();
            pathFinding.SetCheckCanMoveFunc((position =>
                game.Match3Module.CanPutUnit(position) && game.RpgModule.CanPutUnit(position)));
        }

        public override async UniTask Attack()
        {
            var listMachines = Game.instance.RpgModule.GetAllMachines();
            listMachines = listMachines.Where(machine =>
                GridPosition.Distance(machine.GetGridPosition(), GetGridPosition()) <= 5).ToList();
            if (listMachines.Count == 0)
            {
                await AttackBase();
                return;
            }

            // move to attack machine
            listMachines.Sort(new HunterMachineOrder(GetGridPosition()));
            var target = listMachines[0];
            var pathFinding = GetComponent<PathFinding>();
            pathFinding.SetCheckTargetFunc(position =>
            {
                List<GridPosition> listAttackPos = new List<GridPosition>()
                {
                    position + GridPosition.Down,
                    position + GridPosition.Up,
                    position + GridPosition.Right,
                    position + GridPosition.Left,
                };
                return listAttackPos.Find(pos => Game.instance.RpgModule.GetMachine(pos) == target) != null;
            });
            pathFinding.SetEstimatesCostFunc(position => GridPosition.Distance(position, target.GetGridPosition()));
            var path = pathFinding.FindPath(GetGridPosition());
            if (path != null)
            {
                var targetMarker = TargetMarker.CreateObject(target.GetGridPosition());
                var numMoveAvailable = GetComponent<Stat>().GetSpeed();
                await GetComponent<Move>().MoveUsingPath(path, numMoveAvailable);
                targetMarker.Hide();
                if (numMoveAvailable >= path.Count)
                {
                    await GetComponent<SingleTargetAttack>().Attack(target);
                }
                else
                {
                    await GetComponent<IAttack>().Attack<Machine>();
                }
            }
        }

        private async UniTask AttackBase()
        {
            var currentPosition = GetGridPosition();
            if (currentPosition.RowIndex == 0)
            {
                await AttackPlayerBase();
                return;
            }

            var pathFinding = GetComponent<PathFinding>();
            pathFinding.SetCheckTargetFunc(position => position.RowIndex == 0);
            pathFinding.SetEstimatesCostFunc(position => Math.Abs(position.RowIndex));
            var path = pathFinding.FindPath(GetGridPosition());
            var numMoveRemain = GetComponent<Stat>().GetSpeed();
            await GetComponent<Move>().MoveUsingPath(path, numMoveRemain);
            currentPosition = GetGridPosition();
            if (currentPosition.RowIndex == 0)
            {
                await AttackPlayerBase();
            }
        }
    }
}
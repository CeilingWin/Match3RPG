using System;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Units.Common;
using UnityEngine;

namespace Rpg.Units.Monsters.MonsterBehaviour
{
    public class SniperAttack : MonoBehaviour
    {
        public async UniTask Attack()
        {
            // check move
            var unit = GetComponent<Unit>();
            var boardSize = Game.instance.Match3Module.GetBoardSize();
            var game = Game.instance;
            if (unit.GetGridPosition().RowIndex == boardSize.RowIndex - 1)
            {
                var isFullUnit = true;
                for (var columnIndex = 0; columnIndex < boardSize.ColumnIndex; columnIndex++)
                {
                    var pos = new GridPosition(boardSize.RowIndex - 1, columnIndex);
                    if (game.Match3Module.CanPutUnit(pos) && game.RpgModule.CanPutUnit(pos))
                    {
                        isFullUnit = false;
                        break;
                    }
                }

                if (isFullUnit)
                {
                    var forwardPosition = unit.GetGridPosition() + GridPosition.Down;
                    if (game.Match3Module.CanPutUnit(forwardPosition) && game.RpgModule.CanPutUnit(forwardPosition))
                    {
                        await GetComponent<Move>().MoveTo(forwardPosition);
                    }
                }
            }

            GetComponent<Animator>().Play("Attack1");
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            var bullet = Bullet.Create();
            bullet.SetGridPosition(unit.GetGridPosition());
            while (true)
            {
                var currentPosition = bullet.GetGridPosition();
                if (currentPosition.RowIndex == -1)
                {
                    bullet.Hide();
                    await GetComponent<Monster>().AttackPlayerBase();
                    break;
                }
                var target = Game.instance.RpgModule.GetMachine(currentPosition);
                if (target != null)
                {
                    var damage = GetComponent<Stat>().GetDamage();
                    bullet.Hide();
                    await target.TakeDamage(damage);
                    break;
                }

                await bullet.MoveTo(currentPosition + GridPosition.Down);
            }
        }
    }
}
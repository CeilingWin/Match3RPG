using System;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Ability.Attack;
using Rpg.Ability.Detection;
using Unity.VisualScripting;

namespace Rpg.Units.Monsters
{
    public class Bulldog : Monster
    {
        protected override void Start()
        {
            delayAttack = 1.6f;
            base.Start();
            GetComponent<Stat>().SetStat(6, 1, 1, 6, Int32.MaxValue);
            this.AddComponent<SingleTargetAttack>();
            this.AddComponent<ForwardDetection>().SetForwardDirection(GridPosition.Down);
        }
    }
}
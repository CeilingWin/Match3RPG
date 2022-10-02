using System;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Ability.Attack;
using Rpg.Ability.Detection;
using Rpg.Units.Monsters.MonsterBehaviour;
using Unity.VisualScripting;

namespace Rpg.Units.Monsters
{
    public class Shark : Monster
    {
        protected override void Start()
        {
            delayAttack = 1.2f;
            base.Start();
            GetComponent<Stat>().SetStat(3, 4, 1, 3, Int32.MaxValue);
            this.AddComponent<SingleTargetAttack>();
            this.AddComponent<ForwardDetection>().SetForwardDirection(GridPosition.Down);
            this.AddComponent<BallistaAttack>();
        }

        public override async UniTask Attack()
        {
            await GetComponent<BallistaAttack>().Attack();
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using Rpg.Ability;
using Rpg.Units.Monsters.MonsterBehaviour;
using Unity.VisualScripting;

namespace Rpg.Units.Monsters
{
    public class Octopus : Monster
    {
        protected override void Start()
        {
            delayAttack = 1.6f;
            base.Start();
            GetComponent<Stat>().SetStat(6, 1, 1, 6, Int32.MaxValue);
            this.AddComponent<SniperAttack>();
        }

        public override async UniTask Attack()
        {
            await GetComponent<SniperAttack>().Attack();
        }
    }
}
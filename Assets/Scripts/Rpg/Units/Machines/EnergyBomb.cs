using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enum;
using Rpg.Ability;
using Rpg.Ability.Attack;
using Rpg.Ability.Detection;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class EnergyBomb : Machine
    {
        protected override void Start()
        {
            base.Start();
            material = Material.Energy;
            var detection = this.AddComponent<RangeDetection>();
            detection.SetRange(3);
            GetComponent<Stat>().SetStat(1, 0, Int32.MaxValue, 2, 2);
            this.AddComponent<MultiTargetAttack>();
        }

        public override async UniTask Attack()
        {
            if (GetComponent<Stat>().GetCountDown() == 0)
            {
                await GetComponent<IAttack>().Attack<Monster>();
            }
        }
    }
}
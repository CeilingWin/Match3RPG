using System;
using Cysharp.Threading.Tasks;
using Enum;
using Rpg.Ability;
using Rpg.Ability.Attack;
using Rpg.Ability.Detection;
using Rpg.Effects;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class Paralyzer : Machine
    {
        private int countDownToAttack;
        protected override void Start()
        {
            delayAttack = 0.5f;
            base.Start();
            material = Material.Chemistry;
            var detection = this.AddComponent<RangeDetection>();
            detection.SetRange(3);
            GetComponent<Stat>().SetStat(3, 1, 2, 1, 5);
            this.AddComponent<SingleTargetAttack>().SetEffect(new ChangeSpeedEffect(-2, Int32.MaxValue));
            countDownToAttack = 0;
        }

        public override async UniTask Attack()
        {
            countDownToAttack--;
            if (countDownToAttack == 0)
            {
                await GetComponent<IAttack>().Attack<Monster>();
                countDownToAttack = 2;
            }
        }
    }
}
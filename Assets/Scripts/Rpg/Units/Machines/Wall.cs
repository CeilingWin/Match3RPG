using System;
using Cysharp.Threading.Tasks;
using Enum;
using Rpg.Ability;

namespace Rpg.Units.Machines
{
    public class Wall : Machine
    {
        protected override void Start()
        {
            base.Start();
            material = Material.Biology;
            GetComponent<Stat>().SetStat(10, 0, Int32.MaxValue, 0, 5);
        }

        public override UniTask Attack()
        {
            return UniTask.CompletedTask;
        }
    }
}